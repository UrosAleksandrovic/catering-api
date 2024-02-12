namespace Catering.Application.Aggregates.Expenses;

public class ExpensesFilter : FilterBase
{
    public List<string> CustomerIds { get; set; }
    public DateTimeOffset? DeliveredFrom { get; set; }
    public DateTimeOffset? DeliveredTo { get; set; }

    public ExpensesOrderBy? OrderBy { get; set; }
    public bool IsOrderByDescending { get; set; }
}
