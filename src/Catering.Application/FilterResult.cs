namespace Catering.Application;

public class FilterResult<T>
{
    public IEnumerable<T> Result { get; set; }
    public short PageSize { get; set; }
    public short PageIndex { get; set; }
    public int TotalNumberOfElements { get; set; }

    public static FilterResult<TElement> GetEmpty<TElement>(short pageSize, short pageIndex)
        => new()
        {
            PageIndex = pageIndex,
            PageSize = pageSize,
            TotalNumberOfElements = 0,
            Result = Enumerable.Empty<TElement>()
        };
}
