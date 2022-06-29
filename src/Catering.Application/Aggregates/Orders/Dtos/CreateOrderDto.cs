namespace Catering.Application.Aggregates.Orders.Dtos;

public class CreateOrderDto
{
    public DateTime ExpectedTimeOfDelivery { get; set; }
    public HomeDeliveryInfoDto HomeDeliveryInfo { get; set; }
}
