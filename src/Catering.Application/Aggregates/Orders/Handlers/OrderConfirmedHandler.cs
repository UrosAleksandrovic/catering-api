using Catering.Application.Aggregates.Identites.Abstractions;
using Catering.Application.Aggregates.Orders.Abstractions;
using Catering.Application.Aggregates.Orders.Notifications;
using MediatR;

namespace Catering.Application.Aggregates.Orders.Handlers;

internal class OrderConfirmedHandler : INotificationHandler<OrderConfirmed>
{
    private readonly ICustomerRepository _customerRepository;
    private readonly IOrderRepository _orderRepository;

    public OrderConfirmedHandler(
        ICustomerRepository customerRepository,
        IOrderRepository orderRepository)
    {
        _customerRepository = customerRepository;
        _orderRepository = orderRepository;
    }

    public async Task Handle(OrderConfirmed notification, CancellationToken cancellationToken)
    {
        var customer = await _customerRepository.GetByIdAsync(notification.CustomerId);
        var order = await _orderRepository.GetByIdAsync(notification.OrderId);

        customer.ProcessPayment(order.TotalPrice);

        await _customerRepository.UpdateAsync(customer);
    }
}
