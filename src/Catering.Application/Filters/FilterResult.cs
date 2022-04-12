﻿namespace Catering.Application.Filters;

public class FilterResult<T>
{
    public IEnumerable<T> Result { get; set; }
    public short PageSize { get; set; }
    public short PageIndex { get; set; }
    public short TotalNumberOfPages { get; set; }
}
