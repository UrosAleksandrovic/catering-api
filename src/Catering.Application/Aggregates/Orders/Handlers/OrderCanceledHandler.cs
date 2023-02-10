using Catering.Application.Aggregates.Identities.Abstractions;
using Catering.Application.Aggregates.Orders.Abstractions;
using Catering.Application.Aggregates.Orders.Notifications;
using MediatR;

namespace Catering.Application.Aggregates.Orders.Handlers;

internal class OrderCanceledHandler : INotificationHandler<OrderCanceled>
{
    private readonly ICustomerRepository _customerRepository;
    private readonly IOrderRepository _orderRepository;

    public OrderCanceledHandler(
        ICustomerRepository customerRepository,
        IOrderRepository orderRepository)
    {
        _customerRepository = customerRepository;
        _orderRepository = orderRepository;
    }

    public async Task Handle(OrderCanceled notification, CancellationToken cancellationToken)
    {
        var customer = await  _customerRepository.GetByIdAsync(notification.CustomerId);
        var order = await _orderRepository.GetByIdAsync(notification.OrderId);

        customer.CancelPayment(order.TotalPrice);

        await _customerRepository.UpdateAsync(customer);
    }
}
