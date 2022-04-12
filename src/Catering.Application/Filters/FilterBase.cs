namespace Catering.Application.Filters;

public class FilterBase
{
    public short PageSize { get; set; } = 10;
    public short PageIndex { get; set; } = 1;
}
