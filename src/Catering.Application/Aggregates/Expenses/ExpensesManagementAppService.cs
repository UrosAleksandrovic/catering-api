using AutoMapper;
using Catering.Application.Aggregates.Expenses.Abstractions;
using Catering.Application.Aggregates.Expenses.Dtos;
using Catering.Application.Aggregates.Expenses.Notifications;
using Catering.Application.Aggregates.Menus.Requests;
using Catering.Domain.Aggregates.Identity;
using Catering.Domain.Builders;
using MediatR;

namespace Catering.Application.Aggregates.Expenses;

internal class ExpensesManagementAppService : IExpensesManagementAppService
{
    private readonly IExpensesRepository _expensesRepository;
    private readonly IMapper _mapper;
    private readonly IMediator _publisher;

    public ExpensesManagementAppService(
        IExpensesRepository expensesRepository,
        IMapper mapper,
        IMediator publisher)
    {
        _expensesRepository = expensesRepository;
        _mapper = mapper;
        _publisher = publisher;
    }

    public async Task<Guid> CreateAsync(CreateExpenseDto createExpense)
    {
        var expenseToCreate = new ExpenseBuilder()
            .HasMenuAndCustomer(createExpense.MenuId, createExpense.CustomerId)
            .HasPriceAndDate(createExpense.Price, createExpense.DeliveredOn)
            .HasNote(createExpense.Note)
            .Build();

        var createdExpense = await _expensesRepository.CreateAsync(expenseToCreate);

        await _publisher.Publish(new ExpenseCreated
        {
            CustomerId = createdExpense.CustomerId,
            Price = createdExpense.Price
        });

        return createdExpense.Id;
    }

    public async Task<ExpenseInfoDto> GetByIdAsync(Guid id, string requestorId)
    {
        var expense = await _expensesRepository.GetByIdAsync(id);

        var requestor = await _publisher.Send(new GetIdentityById { Id = requestorId });

        if (requestor.Role.IsAdministrator())
            return _mapper.Map<ExpenseInfoDto>(expense);

        return expense.CustomerId == requestorId ? _mapper.Map<ExpenseInfoDto>(expense) : null;
    }

    public async Task<FilterResult<ExpenseInfoDto>> GetFilteredAsync(ExpensesFilter filters)
    {
        var result = FilterResult<ExpenseInfoDto>.GetEmpty<ExpenseInfoDto>(filters.PageIndex, filters.PageSize);

        var (expenses, totalCount) = await _expensesRepository.GetFilteredAsync(filters);
        result.TotalNumberOfElements = totalCount;
        result.Result = _mapper.Map<IEnumerable<ExpenseInfoDto>>(expenses);
        return result;
    }

    public async Task UpdateAsync(Guid id, UpdateExpenseDto updateExpense)
    {
        var expense = await _expensesRepository.GetByIdAsync(id);
        if (expense == default)
            throw new KeyNotFoundException();

        if (!string.IsNullOrWhiteSpace(updateExpense.Note))
            expense.AddNote(updateExpense.Note);

        var expenseUpdated = new ExpenseUpdated
        {
            CustomerId = expense.CustomerId,
            NewPrice = updateExpense.Price,
            PreviousPrice = expense.Price
        };

        expense.UpdatePrice(updateExpense.Price);
        expense.UpdateDeliveredOn(updateExpense.DeliveredOn);

        await _expensesRepository.UpdateAsync(expense);
        await _publisher.Publish(expenseUpdated);
    }
}
