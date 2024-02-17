using Ardalis.GuardClauses;
using Catering.Domain.Aggregates.Identity;
using Catering.Domain.Aggregates.Order;
using Catering.Domain.Builders;
using Catering.Domain.Services.Abstractions;

namespace Catering.Domain.Services;

public class OrderingService : IOrderingService
{
    public Order PlaceOrder(ICustomer customer, IBuilder<Order> orderBuilder)
    {
        Guard.Against.Default(customer);
        Guard.Against.Default(orderBuilder);

        var order = orderBuilder.Build();

        customer.ReserveAssets(order.TotalPrice);

        return order;
    }

    public void ConfirmOrder(ICustomer customer, Order order)
    {
        Guard.Against.Default(customer);
        Guard.Against.Default(order);

        customer.ProcessPayment(order.TotalPrice);

        order.ConfirmOrder();
    }

    public void CancelOrder(ICustomer customer, Order order)
    {
        Guard.Against.Default(customer);
        Guard.Against.Default(order);

        customer.CancelPayment(order.TotalPrice);

        order.CancelOrder();
    }
}
