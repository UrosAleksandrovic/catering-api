namespace Catering.Application.Aggregates.Carts.Dtos;

public class CartItemInfoDto
{
    public Guid ItemId { get; set; }
    public string Name { get; set; }
    public string Description { get; set; }
    public decimal Price { get; set; }
    public string Note { get; set; }
    public int Quantity { get; set; }
}
