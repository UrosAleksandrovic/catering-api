namespace Catering.Application.Aggregates.Orders.Dtos;

public class CreateOrderDto
{
    public DateTimeOffset ExpectedTimeOfDelivery { get; set; }
    public HomeDeliveryInfoDto HomeDeliveryInfo { get; set; }
}
