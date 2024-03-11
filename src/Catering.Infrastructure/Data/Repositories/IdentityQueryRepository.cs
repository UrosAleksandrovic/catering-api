using AutoMapper;
using Catering.Application;
using Catering.Application.Aggregates.Identities;
using Catering.Application.Aggregates.Identities.Abstractions;
using Catering.Domain.Aggregates.Identity;
using Catering.Infrastructure.EFUtility;
using Microsoft.EntityFrameworkCore;

namespace Catering.Infrastructure.Data.Repositories;

internal class IdentityQueryRepository<TIdentity> : IIdentityQueryRepository<TIdentity>
    where TIdentity : Identity
{
    private readonly IDbContextFactory<CateringDbContext> _dbContextFactory;
    private readonly IMapper _mapper;

    public IdentityQueryRepository(IDbContextFactory<CateringDbContext> dbContextFactory, IMapper mapper)
    {
        _dbContextFactory = dbContextFactory;
        _mapper = mapper;
    }

    public async Task<TDto> GetByIdAsync<TDto>(string id)
    {
        var dbContext = await _dbContextFactory.CreateDbContextAsync();

        var query = dbContext.Set<TIdentity>().AsNoTracking().Where(x => x.Id == id);

        return await _mapper.ProjectTo<TDto>(query).SingleOrDefaultAsync();
    }

    public async Task<PageBase<TDto>> GetPageAsync<TDto>(IdentityFilter filter)
    {
        var dbContext = await _dbContextFactory.CreateDbContextAsync();

        var query = dbContext.Set<TIdentity>().AsNoTracking();
        query = ApplyFilter(query, filter);
        query = ApplyOrderBy(query, filter);

        return new(await _mapper.ProjectTo<TDto>(query.Paginate(filter)).ToListAsync(), await query.CountAsync());
    }

    private static IQueryable<TIdentity> ApplyFilter(IQueryable<TIdentity> query, IdentityFilter filter)
    {
        if (filter == null)
        {
            return query;
        }

        if (!string.IsNullOrWhiteSpace(filter.Email))
        {
            query = query.Where(x => x.Email.Contains(filter.Email));
        }

        if (!string.IsNullOrWhiteSpace(filter.FirstName))
        {
            query = query.Where(x => EF.Functions.Like(x .FullName.FirstName, $"%{filter.FirstName}%"));
        }

        if (!string.IsNullOrWhiteSpace(filter.LastName))
        {
            query = query.Where(x => EF.Functions.Like(x.FullName.LastName, $"%{filter.LastName}%"));
        }

        if (filter.Role != null)
        {
            query = query.Where(x => x.Role == filter.Role);
        }

        if (filter.IsExternal != null)
        {
            query = query.Where(x => x.IsExternal == filter.IsExternal);
        }

        return query;
    }

    public static IQueryable<TIdentity> ApplyOrderBy(IQueryable<TIdentity> query, IdentityFilter filter)
    {
        if (filter == null || filter.OrderBy == null)
        {
            return query;
        }

        return filter switch
        {
            { OrderBy: IdentitiesOrderBy.FirstName, IsOrderByDescending: false } => query.OrderBy(i => i.FullName.FirstName),
            { OrderBy: IdentitiesOrderBy.FirstName, IsOrderByDescending: true } => query.OrderByDescending(i => i.FullName.FirstName),

            { OrderBy: IdentitiesOrderBy.Email, IsOrderByDescending: false } => query.OrderBy(i => i.FullName.FirstName),
            { OrderBy: IdentitiesOrderBy.Email, IsOrderByDescending: true } => query.OrderByDescending(i => i.FullName.FirstName),

            _ => query.OrderByDescending(i => i.FullName.FirstName),
        };
    }
}
