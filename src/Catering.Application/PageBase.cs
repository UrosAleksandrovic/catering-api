namespace Catering.Application;

public record PageBase<TResult>(List<TResult> Data, int TotalCount)
{
    public static PageBase<TResult> Empty() => new([], 0);
}
