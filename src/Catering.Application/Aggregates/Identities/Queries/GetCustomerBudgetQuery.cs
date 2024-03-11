using Catering.Application.Aggregates.Identities.Abstractions;
using Catering.Application.Aggregates.Identities.Dtos;
using Catering.Application.Results;
using MediatR;

namespace Catering.Application.Aggregates.Identities.Queries;

public record GetCustomerBudgetQuery(string CustomerId) : IRequest<Result<CustomerBudgetInfoDto>>;

public class GetCustomerBudgetQueryHandler(ICustomerQueryRepository queryRepository) 
    : IRequestHandler<GetCustomerBudgetQuery, Result<CustomerBudgetInfoDto>>
{
    private readonly ICustomerQueryRepository _queryRepository = queryRepository;

    public async Task<Result<CustomerBudgetInfoDto>> Handle(
        GetCustomerBudgetQuery request, 
        CancellationToken cancellationToken)
        => Result.Success(await _queryRepository.GetCustomerBudgetAsync(request.CustomerId));
}
