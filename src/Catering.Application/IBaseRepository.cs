namespace Catering.Application;

public interface IBaseRepository<T>
{
    Task<T> CreateAsync(T entity);
    Task<T> UpdateAsync(T entity);
    Task<T> GetByIdAsync<TKey>(TKey key);
}
