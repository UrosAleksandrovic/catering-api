using Catering.Domain.Aggregates.Order;

namespace Catering.Application.Aggregates.Orders.Dtos;

public class ListOrderInfoDto
{
    public long Id { get; set; }
    public bool IsHomeDelivery { get; set; }
    public HomeDeliveryInfoDto HomeDeliveryInfo { get; set; }
    public DateTimeOffset ExpectedOn { get; set; }
    public OrderStatus Status { get; set; }
    public decimal TotalSumToPay { get; set; }
}
