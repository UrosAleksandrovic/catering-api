using Catering.Domain.Aggregates.Cart;

namespace Catering.Application.Aggregates.Carts.Abstractions;

public interface ICartRepository : IBaseCrudRepository<Cart>
{
    Task<Cart> GetByCustomerIdAsync(string customerId);
    Task DeleteByCustomerIdAsync(string customerId);
    Task DeleteItemsWithMenuAsync(Guid menuId);
}
