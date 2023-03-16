using Catering.Domain.Builders;
using Catering.Domain.Entities.IdentityAggregate;
using Catering.Domain.Entities.OrderAggregate;

namespace Catering.Domain.Services.Abstractions;

public interface IOrderingService
{
    Order PlaceOrder(ICustomer customer, IBuilder<Order> orderBuilder);

    void ConfirmOrder(ICustomer customer, Order order);

    void CancelOrder(ICustomer customer, Order order);
}
