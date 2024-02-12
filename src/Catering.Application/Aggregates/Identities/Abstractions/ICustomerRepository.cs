using Catering.Domain.Entities.IdentityAggregate;

namespace Catering.Application.Aggregates.Identities.Abstractions;

public interface ICustomerRepository : IBaseCrudRepository<Customer>
{
    Task<Customer> GetByIdentityEmailAsync(string email);
    Task<Customer> GetFullByIdAsync(string id);
    Task<(IEnumerable<Customer>, int)> GetFilteredInternalCustomersAsync(CustomersFilter filter);
    Task<(IEnumerable<Customer>, int)> GetFilteredExternalCustomersAsync(CustomersFilter filter);
    Task ResetBudgetAsync(double newBudget);
}
