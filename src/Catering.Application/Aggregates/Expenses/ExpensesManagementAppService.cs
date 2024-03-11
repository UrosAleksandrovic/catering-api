using AutoMapper;
using Catering.Application.Aggregates.Expenses.Abstractions;
using Catering.Application.Aggregates.Expenses.Dtos;
using Catering.Application.Aggregates.Expenses.Notifications;
using Catering.Application.Results;
using Catering.Application.Validation;
using Catering.Domain.Aggregates.Expense;
using Catering.Domain.Builders;
using MediatR;

namespace Catering.Application.Aggregates.Expenses;

internal class ExpensesManagementAppService : IExpensesManagementAppService
{
    private readonly IExpensesRepository _expensesRepository;
    private readonly IValidationProvider _validationProvider;
    private readonly IMediator _publisher;

    public ExpensesManagementAppService(
        IExpensesRepository expensesRepository,
        IValidationProvider validationProvider,
        IMediator publisher)
    {
        _expensesRepository = expensesRepository;
        _validationProvider = validationProvider;
        _publisher = publisher;
    }

    public async Task<Result<Guid>> CreateAsync(CreateExpenseDto createExpense)
    {
        if (await _validationProvider.ValidateModelAsync(createExpense) is var valResult && !valResult.IsSuccess)
            return Result.From<Guid>(valResult);

        var expenseToCreate = ConstructExpense(createExpense);
        var createdExpense = await _expensesRepository.CreateAsync(expenseToCreate);

        await _publisher.Publish(new ExpenseCreated(createdExpense.CustomerId, createdExpense.Price));

        return Result.Success(createdExpense.Id);
    }

    public async Task<Result> UpdateAsync(Guid id, UpdateExpenseDto updateRequest)
    {
        if (await _validationProvider.ValidateModelAsync(updateRequest) is var valResult && !valResult.IsSuccess)
            return valResult;

        var expense = await _expensesRepository.GetByIdAsync(id);
        if (expense == default)
            return Result.NotFound();

        var updateEvet = UpdateExpense(expense, updateRequest);

        await _expensesRepository.UpdateAsync(expense);
        await _publisher.Publish(updateEvet);

        return Result.Success();
    }

    private ExpenseUpdated UpdateExpense(Expense expense, UpdateExpenseDto updateExpense)
    {
        if (!string.IsNullOrWhiteSpace(updateExpense.Note))
            expense.AddNote(updateExpense.Note);

        expense.UpdatePrice(updateExpense.Price);
        expense.UpdateDeliveredOn(updateExpense.DeliveredOn);

        return new(expense.CustomerId, expense.Price, updateExpense.Price);
    }

    private static Expense ConstructExpense(CreateExpenseDto createExpense)
        => new ExpenseBuilder()
            .HasMenuAndCustomer(createExpense.MenuId, createExpense.CustomerId)
            .HasPriceAndDate(createExpense.Price, createExpense.DeliveredOn)
            .HasNote(createExpense.Note)
            .Build();
}
