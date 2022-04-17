namespace Catering.Application.Aggregates.Items;

public class ItemsFilter : FilterBase
{
    public IEnumerable<string> Categories { get; set; }
    public decimal TopPrice { get; set; }
    public decimal BottomPrice { get; set; }
    public string MenuId { get; set; }
}
