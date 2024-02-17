using Ardalis.GuardClauses;

namespace Catering.Domain.Aggregates.Expense;

public class Expense
{
    public Guid Id { get; private set; }
    public Guid MenuId { get; private set; }
    public string CustomerId { get; private set; }
    public DateTime CreatedOn { get; private set; }
    public DateTime DeliveredOn { get; private set; }
    public decimal Price { get; private set; }
    public string Note { get; private set; }

    public Expense(
        Guid menuId,
        string customerId,
        DateTime deliveredOn,
        decimal price)
    {
        Guard.Against.Default(menuId);
        Guard.Against.NullOrWhiteSpace(customerId);
        Guard.Against.Default(deliveredOn);
        Guard.Against.NegativeOrZero(price);

        Id = Guid.NewGuid();
        CreatedOn = DateTime.UtcNow;
        MenuId = menuId;
        CustomerId = customerId;
        DeliveredOn = deliveredOn;
        Price = price;
    }

    public void AddNote(string note)
    {
        Note = note;
    }

    public void UpdatePrice(decimal newPrice)
    {
        Guard.Against.NegativeOrZero(newPrice);

        Price = newPrice;
    }

    public void UpdateDeliveredOn(DateTime newDeliveredOn)
    {
        Guard.Against.Default(newDeliveredOn);

        DeliveredOn = newDeliveredOn;
    }
}
