using Catering.Application.Aggregates.Identites.Abstractions;
using Catering.Domain.Entities.IdentityAggregate;
using Microsoft.EntityFrameworkCore;

namespace Catering.Infrastructure.Data.Repositories;

internal class CustomerRepository : IdentityRepository<Customer>, ICustomerRepository
{
    public CustomerRepository(IDbContextFactory<CateringDbContext> dbContextFactory) 
        : base(dbContextFactory) { }
}
