using Catering.Domain.Entities.CartAggregate;

namespace Catering.Application.Aggregates.Carts.Abstractions;

public interface ICartRepository : IBaseRepository<Cart>
{
    public Task<Cart> GetByCustomerIdAsync(string customerId);
    public Task DeleteAsync(string customerId);
}
