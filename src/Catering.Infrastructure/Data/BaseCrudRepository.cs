using Catering.Application;
using Microsoft.EntityFrameworkCore;

namespace Catering.Infrastructure.Data;

internal class BaseCrudRepository<T, TContext> : IBaseCrudRepository<T> 
    where TContext : DbContext 
    where T : class
{
    protected readonly IDbContextFactory<TContext> _dbContextFactory;

    protected BaseCrudRepository(IDbContextFactory<TContext> dbContextFactory)
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

        dbContext.Set<T>().Update(entity);

        //dbContext.Set<T>().Attach(entity);

        //var entry = dbContext.Entry(entity);
        //entry.State = EntityState.Modified;

        //foreach (var navigation in dbContext.Entry(entity).Navigations)
        //{
        //    navigation.IsModified = true;
        //    navigation.IsLoaded = true;
        //}
        //dbContext.ChangeTracker.DetectChanges();

        await dbContext.SaveChangesAsync();

        return entity;
    }

    public async Task HardDeleteAsync(T entity)
    {
        using var dbContext = await _dbContextFactory.CreateDbContextAsync();

        dbContext.Set<T>().Remove(entity);
        await dbContext.SaveChangesAsync();
    }
}
