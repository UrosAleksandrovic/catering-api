namespace Catering.Application;

public interface IBaseRepository<T>
{
    Task<T> CreateAsync(T entity);
    Task<T> UpdateAsync(T entity);
    Task<T> GetAsync(short pageSize, short pageIndex);
    Task<T> GetByIdAsync<TKey>(TKey key);

    IPersistanceCommander UnitOfWork { get; }
}
