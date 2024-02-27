using Catering.Application.Aggregates.Orders.Abstractions;
using Catering.Application.Aggregates.Orders.Dtos;
using Catering.Application.Aggregates.Orders.Requests;
using Catering.Application.Results;
using Catering.Domain.Aggregates.Identity;
using MediatR;

namespace Catering.Application.Aggregates.Orders.Queries;

public record GetOrderByIdQuery(long OrderId, string RequestorId) : IRequest<Result<OrderInfoDto>>;

public class GetOrderByIdQueryHandler(IOrdersQueryRepository queryRepository, IMediator publisher) 
    : IRequestHandler<GetOrderByIdQuery, Result<OrderInfoDto>>
{
    private readonly IOrdersQueryRepository _queryRepository = queryRepository;
    private readonly IMediator _publisher = publisher;

    public async Task<Result<OrderInfoDto>> Handle(
        GetOrderByIdQuery request,
        CancellationToken cancellationToken)
    {
        var order = await _queryRepository.GetByIdAsync(request.OrderId);
        if (order == default)
            return Result.NotFound();

        var customerRequest = new GetOrderCustomer { CustomerId = request.RequestorId };
        var customer = await _publisher.Send(customerRequest);

        if (customer.Identity.Role.IsClientEmployee() && order.CustomerId != customer.IdentityId)
            return Result.NotFound();

        return Result.Success(order);
    }
}
