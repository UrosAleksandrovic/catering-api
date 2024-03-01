using Catering.Application.Results;

namespace Catering.Application.Filtering;

public class FilterResult<T> : Result<IEnumerable<T>>
{
    public short PageSize { get; set; }
    public short PageIndex { get; set; }
    public int TotalNumberOfElements { get; set; }

    public static FilterResult<TElement> Empty<TElement>(short pageSize, short pageIndex)
        => new()
        {
            PageIndex = pageIndex,
            PageSize = pageSize,
            TotalNumberOfElements = 0,
            Value = Enumerable.Empty<TElement>(),
            IsSuccess = true,
            ErrorCodes = Array.Empty<string>(),
            Type = null
        };

    public static FilterResult<TElement> Empty<TElement>(FilterBase filter)
        => Empty<TElement>(filter.PageSize, filter.PageIndex);
}
