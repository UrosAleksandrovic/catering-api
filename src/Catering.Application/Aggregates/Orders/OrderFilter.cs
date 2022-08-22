namespace Catering.Application.Aggregates.Orders;

public class OrdersFilter : FilterBase
{
    public string CustomerId { get; set; }
    public Guid? MenuId { get; set; }

    public OrdersFilter() { }

    public OrdersFilter(OrdersFilter filter)
    {
        PageSize = filter.PageSize;
        PageIndex = filter.PageIndex;
        CustomerId = filter.CustomerId;
        MenuId = filter.MenuId;
    }
}
