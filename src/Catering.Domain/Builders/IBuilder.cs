namespace Catering.Domain.Builders;

public interface IBuilder<out T> 
{
    T Build();
    void Reset();
}
