namespace Catering.Application.Aggregates.Carts.Dtos;

public class AddItemToCartDto
{
    public Guid ItemId { get; set; }
    public int Quantity { get; set; } = 1;
    public string Note { get; set; } = null;
}
