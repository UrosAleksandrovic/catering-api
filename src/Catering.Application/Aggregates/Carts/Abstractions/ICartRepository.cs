using Catering.Domain.Entities.CartAggregate;

namespace Catering.Application.Aggregates.Carts.Abstractions;

public interface ICartRepository : IBaseCrudRepository<Cart>
{
    Task<Cart> GetByCustomerIdAsync(string customerId);
    Task DeleteAsync(string customerId);
}
