namespace Catering.Application.Aggregates.Orders.Dtos;

public class OrderItemInfoDto
{
    public Guid ItemId { get; set; }
    public string PriceSnapshot { get; set; }
    public string Note { get; set; }
    public string Quantity { get; set; }
}
