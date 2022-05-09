using Catering.Application.Aggregates.Menus.Abstractions;
using Catering.Domain.Entities.MenuAggregate;
using Microsoft.EntityFrameworkCore;

namespace Catering.Infrastructure.Data.Repositories;

internal class MenuRepository : BaseCrudRepository<Menu, CateringDbContext>, IMenuRepository
{
    public MenuRepository(IDbContextFactory<CateringDbContext> dbContextFactory)
        : base(dbContextFactory) { }
}
