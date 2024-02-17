using Catering.Domain.Aggregates.Order;

namespace Catering.Application.Aggregates.Orders;

public class OrdersFilter : FilterBase
{
    public string CustomerId { get; set; }
    public Guid? MenuId { get; set; }
    public DateTimeOffset? DeliveredOn { get; set; }
    public decimal? TopPrice { get; set; }
    public decimal? BottomPrice { get; set; }
    public List<OrderStatus> Statuses { get; set; }
    public bool? IsHomeDelivery { get; set; }

    public OrdersOrderBy? OrderBy { get; set; }
    public bool IsOrderByDescending { get; set; }

    public OrdersFilter() { }

    public OrdersFilter(OrdersFilter filter)
    {
        PageSize = filter.PageSize;
        PageIndex = filter.PageIndex;
        CustomerId = filter.CustomerId;
        MenuId = filter.MenuId;
    }
}
