using Catering.Domain.Aggregates.Cart;

namespace Catering.Application.Aggregates.Carts.Abstractions;

public interface ICartRepository : IBaseCrudRepository<Cart>
{
    Task<Cart> GetByCustomerIdAsync(string customerId);
    Task DeleteAsync(string customerId);

    /// <summary>
    /// Entity framework does not track the items that are deleted from the collection when attached
    /// Only those that were already there.
    /// </summary>
    /// <see cref="https://github.com/dotnet/efcore/issues/17626"/>
    Task CleanUpDeletedItemsAsync(Cart cart);
}
