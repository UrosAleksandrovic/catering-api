using Catering.Domain.Aggregates.Order;

namespace Catering.Application.Aggregates.Orders.Dtos;

public class OrderInfoDto
{
    public long Id { get; set; }
    public HomeDeliveryInfoDto HomeDeliveryInfo { get; set; }
    public List<OrderItemInfoDto> Items { get; set; }
    public DateTimeOffset ExpectedOn { get; set; }
    public DateTimeOffset OrderedAt { get; set; }
    public OrderStatus Status { get; set; }
    public decimal TotalSumToPay { get; set; }
    public Guid MenuId { get; set; }
    public string CustomerId { get; set; }
}
