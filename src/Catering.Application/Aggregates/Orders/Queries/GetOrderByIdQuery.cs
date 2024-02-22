using Catering.Application.Aggregates.Orders.Abstractions;
using Catering.Application.Aggregates.Orders.Dtos;
using Catering.Application.Aggregates.Orders.Requests;
using Catering.Domain.Aggregates.Identity;
using MediatR;

namespace Catering.Application.Aggregates.Orders.Queries;

public record GetOrderByIdQuery(long OrderId, string RequestorId) : IRequest<OrderInfoDto>;

public class GetOrderByIdQueryHandler(IOrdersQueryRepository queryRepository, IMediator publisher) 
    : IRequestHandler<GetOrderByIdQuery, OrderInfoDto>
{
    private readonly IOrdersQueryRepository _queryRepository = queryRepository;
    private readonly IMediator _publisher = publisher;

    public async Task<OrderInfoDto> Handle(
        GetOrderByIdQuery request,
        CancellationToken cancellationToken)
    {
        var order = await _queryRepository.GetByIdAsync(request.OrderId);
        if (order == default)
            throw new KeyNotFoundException();

        var customerRequest = new GetOrderCustomer { CustomerId = request.RequestorId };
        var customer = await _publisher.Send(customerRequest);

        if (customer.Identity.Role.IsClientEmployee() && order.CustomerId != customer.IdentityId)
            return null;

        return order;
    }
}
