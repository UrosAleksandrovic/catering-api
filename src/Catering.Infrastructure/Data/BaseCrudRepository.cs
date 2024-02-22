using Catering.Application;
using Microsoft.EntityFrameworkCore;

namespace Catering.Infrastructure.Data;

internal class BaseCrudRepository<T, TContext> : IBaseCrudRepository<T>
    where TContext : DbContext 
    where T : class
{
    protected TContext _dbContext;

    protected BaseCrudRepository(TContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<T> CreateAsync(T entity)
    {
        await _dbContext.Set<T>().AddAsync(entity);
        await _dbContext.SaveChangesAsync();

        return entity;
    }

    public async Task<T> GetByIdAsync(params object[] key) 
        => await _dbContext.Set<T>().FindAsync(key);

    public async Task<T> UpdateAsync(T entity)
    {
        _dbContext.Update(entity);
        await _dbContext.SaveChangesAsync();

        return entity;
    }

    public async Task HardDeleteAsync(T entity)
    {
        _dbContext.Set<T>().Remove(entity);
        await _dbContext.SaveChangesAsync();
    }
}
