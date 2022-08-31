using Catering.Domain.Entities.IdentityAggregate;

namespace Catering.Application.Aggregates.Identites.Abstractions;

public interface ICustomerRepository : IBaseCrudRepository<Customer>
{
    Task<(IEnumerable<Customer>,int)> GetFilteredAsync(CustomersFilter filter);
    Task<Customer> GetByIdentityEmailAsync(string email);
    Task<Customer> GetFullByIdAsync(string id);
}
