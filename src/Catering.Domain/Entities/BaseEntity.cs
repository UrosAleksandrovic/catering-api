namespace Catering.Domain.Entities;

public class BaseEntity<T>
{
    public T Id { get; protected set; }
}
