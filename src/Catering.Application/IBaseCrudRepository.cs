namespace Catering.Application;

public interface IBaseCrudRepository<T>
{
    Task<T> CreateAsync(T entity);
    Task<T> UpdateAsync(T entity);
    Task<T> GetByIdAsync<TKey>(TKey key);
    Task HardDeleteAsync(T entity);
}
