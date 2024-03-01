using Catering.Application.Aggregates.Identities.Abstractions;
using Catering.Application.Aggregates.Identities.Dtos;
using Catering.Application.Results;
using Catering.Domain;
using MediatR;

namespace Catering.Application.Aggregates.Identities.Queries;

public record GetCustomerMonthlySendingsQuery(YearAndMonth Target) : IRequest<Result<List<CustomerMonthlySpendingDto>>>;

public class GetCustomerMonthlySendingsQueryHandler(ICustomerReportsRepository customerRepository)
        : IRequestHandler<GetCustomerMonthlySendingsQuery, Result<List<CustomerMonthlySpendingDto>>>
{
    private readonly ICustomerReportsRepository _customerRepository = customerRepository;

    public async Task<Result<List<CustomerMonthlySpendingDto>>> Handle(
        GetCustomerMonthlySendingsQuery request,
        CancellationToken cancellationToken)
    {
        var today = DateTimeOffset.UtcNow;
        var target = request?.Target == null ? new YearAndMonth(today.Year, today.Month) : request.Target;

        return Result.Success(await _customerRepository.GetMonthlySendingReportAsync(target));
    }
}
