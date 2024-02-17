namespace Catering.Application.Aggregates.Carts.Dtos;

public class CartInfoDto
{
    public string Id { get; set; }
    public string CustomerId { get; set; }
    public Guid? MenuId { get; set; }
    public IEnumerable<CartItemInfoDto> Items { get; set; }
}
