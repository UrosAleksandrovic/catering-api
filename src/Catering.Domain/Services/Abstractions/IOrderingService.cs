using Catering.Domain.Aggregates.Identity;
using Catering.Domain.Aggregates.Order;
using Catering.Domain.Builders;

namespace Catering.Domain.Services.Abstractions;

public interface IOrderingService
{
    Order PlaceOrder(ICustomer customer, IBuilder<Order> orderBuilder);

    void ConfirmOrder(ICustomer customer, Order order);

    void CancelOrder(ICustomer customer, Order order);
}
