using Catering.Domain.Entities.IdentityAggregate;

namespace Catering.Application.Aggregates.Identites.Abstractions;

public interface ICustomerRepository : IBaseRepository<Customer>, IIdentityRepository<Customer>
{

}
