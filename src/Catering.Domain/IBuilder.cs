using Catering.Domain.Entities;

namespace Catering.Domain;

public interface IBuilder<T> where T : BaseEntity
{
    T Build();
}
