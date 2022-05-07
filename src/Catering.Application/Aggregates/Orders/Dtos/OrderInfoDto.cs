using Catering.Domain.Entities.OrderAggregate;

namespace Catering.Application.Aggregates.Orders.Dtos;

public class OrderInfoDto
{
    public long Id { get; set; }
    public HomeDeliveryInfoDto HomeDeliveryInfo { get; set; }
    public List<OrderItemInfoDto> Items { get; set; }
    public DateTime ExpectedOn { get; set; }
    public OrderStatus Status { get; set; }
    public decimal TotalSumToPay { get; set; }
    public Guid MenuId { get; set; }
}
