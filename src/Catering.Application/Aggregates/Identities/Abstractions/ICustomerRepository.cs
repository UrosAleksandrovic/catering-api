using Catering.Domain.Aggregates.Identity;

namespace Catering.Application.Aggregates.Identities.Abstractions;

public interface ICustomerRepository : IBaseCrudRepository<Customer>
{
    Task<Customer> GetByIdentityEmailAsync(string email);
    Task<Customer> GetFullByIdAsync(string id);
    Task ResetBudgetAsync(decimal newBudget);
    Task CreateAsync(Customer customer, Identity identity);
}
