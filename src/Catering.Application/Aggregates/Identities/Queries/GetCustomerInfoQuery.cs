using Catering.Application.Aggregates.Identities.Abstractions;
using Catering.Application.Aggregates.Identities.Dtos;
using Catering.Application.Results;
using MediatR;

namespace Catering.Application.Aggregates.Identities.Queries;

public record GetCustomerInfoQuery(string CustomerId) : IRequest<Result<CustomerInfoDto>>;

public class GetCustomerInfoQueryHandler(ICustomerQueryRepository queryRepo) 
    : IRequestHandler<GetCustomerInfoQuery, Result<CustomerInfoDto>>
{
    private readonly ICustomerQueryRepository queryRepository = queryRepo;

    public async Task<Result<CustomerInfoDto>> Handle(
        GetCustomerInfoQuery request,
        CancellationToken cancellationToken)
        => Result.Success(await queryRepository.GetCustomerInfoAsync(request.CustomerId));

}
