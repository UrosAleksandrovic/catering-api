using Catering.Application.Aggregates.Expenses.Abstractions;
using Catering.Application.Aggregates.Expenses.Dtos;
using Catering.Application.Aggregates.Menus.Requests;
using Catering.Application.Results;
using Catering.Domain.Aggregates.Identity;
using MediatR;

namespace Catering.Application.Aggregates.Expenses.Queries;

public record GetExpenseByIdQuery(Guid Id, string RequestorId) : IRequest<Result<ExpenseInfoDto>>;

internal class GetExpenseByIdQueryHandler(IExpensesQueryRepository queryRepository, IMediator publisher) 
    : IRequestHandler<GetExpenseByIdQuery, Result<ExpenseInfoDto>>
{
    private readonly IExpensesQueryRepository _queryRepository = queryRepository;
    private readonly IMediator _publisher = publisher;

    public async Task<Result<ExpenseInfoDto>> Handle(GetExpenseByIdQuery request, CancellationToken cancellationToken)
    {
        var requestor = await _publisher.Send(new GetIdentityById(request.RequestorId));
        if (requestor.Role.IsAdministrator())
            return Result.NotFound();

        var expense = await _queryRepository.GetByIdAsync(request.Id);

        return expense.CustomerId == request.RequestorId ? Result.Success(expense) : Result.NotFound();
    }
}
