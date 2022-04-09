using Catering.Domain.Builders;
using Catering.Domain.Entities.OrderAggregate;
using Catering.Domain.Entities.UserAggregate;

namespace Catering.Domain.Services.Abstractions;

public interface IOrderingService
{
    public Order PlaceOrder(User user, IBuilder<Order> orderBuilder);

    public void ConfirmOrder(User user, Order order);

    public void CancelOrder(User user, Order order);
}
