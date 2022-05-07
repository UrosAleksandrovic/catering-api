using Catering.Application.Aggregates.Items.Requests;
using Catering.Application.Aggregates.Orders.Abstractions;
using Catering.Domain.Entities.OrderAggregate;
using MediatR;

namespace Catering.Application.Aggregates.Items.Handlers;

internal class GetActiveOrdersOfItemsHandler : IRequestHandler<GetActiveOrdersOfItem, List<Order>>
{
    private readonly IOrderRepository _orderRepository;

    public GetActiveOrdersOfItemsHandler(IOrderRepository repository)
    {
        _orderRepository = repository;
    }

    public Task<List<Order>> Handle(GetActiveOrdersOfItem request, CancellationToken cancellationToken)
        => _orderRepository.GetActiveOrdersByItemAsync(request.ItemId);
}
