using Catering.Domain.Entities;

namespace Catering.Domain.Builders;

public interface IBuilder<T, TKey> where T : BaseEntity<TKey>
{
    T Build();
    void Reset();
}
