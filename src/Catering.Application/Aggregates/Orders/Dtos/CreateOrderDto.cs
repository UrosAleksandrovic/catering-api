namespace Catering.Application.Aggregates.Orders.Dtos;

public class CreateOrderDto
{
    public string CustomerId { get; set; }
    public DateTime ExpectedTimeOfDelivery { get; set; }
    public HomeDeliveryInfoDto HomeDeliveryInfo { get; set; }
}
