using Catering.Application;
using Catering.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Catering.Infrastructure.Data;

internal class BaseRepository<T, TContext> : IBaseRepository<T> 
    where TContext : DbContext 
    where T : class
{
    protected readonly IDbContextFactory<TContext> _dbContextFactory;

    protected BaseRepository(IDbContextFactory<TContext> dbContextFactory)
    {
        _dbContextFactory = dbContextFactory;
    }

    public async Task<T> CreateAsync(T entity)
    {
        using var dbContext = await _dbContextFactory.CreateDbContextAsync();

        await dbContext.Set<T>().AddAsync(entity);
        await dbContext.SaveChangesAsync();

        return entity;
    }

    public async Task<T> GetByIdAsync<TKey>(TKey key)
    {
        using var dbContext = await _dbContextFactory.CreateDbContextAsync();

        var result = await dbContext.Set<T>().FindAsync(key);

        return result;
    }

    public async Task<T> UpdateAsync(T entity)
    {
        using var dbContext = await _dbContextFactory.CreateDbContextAsync();

        dbContext.Update(entity);
        await dbContext.SaveChangesAsync();

        return entity;
    }
}
