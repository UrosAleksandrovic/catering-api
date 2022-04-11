using Ardalis.GuardClauses;
using Catering.Domain.Builders;
using Catering.Domain.Entities.OrderAggregate;
using Catering.Domain.Entities.UserAggregate;
using Catering.Domain.Services.Abstractions;

namespace Catering.Domain.Services;

public class OrderingService : IOrderingService
{
    public Order PlaceOrder(User user, IBuilder<Order> orderBuilder)
    {
        Guard.Against.Default(user);
        Guard.Against.Default(orderBuilder);

        var order = orderBuilder.Build();

        user.ReserveAssets(order.TotalPrice);

        return order;
    }

    public void ConfirmOrder(User user, Order order)
    {
        Guard.Against.Default(user);
        Guard.Against.Default(order);

        user.ProcessPayment(order.TotalPrice);

        order.ConfirmOrder();
    }

    public void CancelOrder(User user, Order order)
    {
        Guard.Against.Default(user);
        Guard.Against.Default(order);

        user.CancelPayment(order.TotalPrice);

        order.CancelOrder();
    }
}
