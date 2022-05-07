namespace Catering.Application.Aggregates.Orders;

public class OrderFilter : FilterBase
{
    public string CustomerId { get; set; }
    public Guid? MenuId { get; set; }
}
