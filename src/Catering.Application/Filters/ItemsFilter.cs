namespace Catering.Application.Filters;

public class ItemsFilter : FilterBase
{
    public IEnumerable<string> Categories { get; set; }
    public decimal TopPrice { get; set; }
    public decimal BottomPrice { get; set; }
}
