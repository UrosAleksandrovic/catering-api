namespace Catering.Application.Aggregates.Items;

public class ItemsFilter : FilterBase
{
    public IEnumerable<string> Categories { get; set; }
    public decimal? TopPrice { get; set; }
    public decimal? BottomPrice { get; set; }
    public Guid MenuId { get; set; }
    public ItemsOrderBy? OrderBy { get; set; }
}
