using Ardalis.GuardClauses;

namespace Catering.Domain.Entities.OrderAggregate;

public class OrderItem
{
    public long OrderId { get; private set; }
    public Guid ItemId { get; private set; }
    public decimal PriceSnapshot { get; private set; }
    public string Note { get; private set;}
    public int Quantity { get; private set; }

    private OrderItem() { }

    public OrderItem(Guid itemId, decimal price, int quantity, string note)
    {
        Guard.Against.NegativeOrZero(price);
        Guard.Against.Negative(Quantity);
        Guard.Against.Default(itemId);

        ItemId = itemId;
        PriceSnapshot = price;
        Note = note;
        Quantity = quantity;
    }
}
