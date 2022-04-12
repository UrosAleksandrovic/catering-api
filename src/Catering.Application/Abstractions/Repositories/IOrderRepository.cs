using Catering.Domain.Entities.OrderAggregate;

namespace Catering.Application.Abstractions.Repositories
{
    public interface IOrderRepository : IBaseRepository<Order>
    {
        List<Order> GetActiveOrdersForItemAsync(Guid itemId);
    }
}
