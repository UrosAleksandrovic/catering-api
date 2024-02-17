using Ardalis.GuardClauses;

namespace Catering.Domain.Aggregates.Order;

public class OrderItem
{
    public long OrderId { get; private set; }
    public Guid ItemId { get; private set; }
    public string NameSnapshot { get; private set; }
    public decimal PriceSnapshot { get; private set; }
    public string Note { get; private set; }
    public int Quantity { get; private set; }

    private OrderItem() { }

    public OrderItem(Guid itemId, decimal price, string name, int quantity, string note)
    {
        Guard.Against.NegativeOrZero(price);
        Guard.Against.Negative(Quantity);
        Guard.Against.Default(itemId);
        Guard.Against.NullOrEmpty(name);

        ItemId = itemId;
        PriceSnapshot = price;
        Note = note;
        Quantity = quantity;
        NameSnapshot = name;
    }
}
