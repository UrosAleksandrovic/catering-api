using Catering.Domain.Builders;
using Catering.Domain.Entities.IdentityAggregate;
using Catering.Domain.Entities.OrderAggregate;

namespace Catering.Domain.Services.Abstractions;

public interface IOrderingService
{
    public Order PlaceOrder(ICustomer customer, IBuilder<Order> orderBuilder);

    public void ConfirmOrder(ICustomer customer, Order order);

    public void CancelOrder(ICustomer customer, Order order);
}
