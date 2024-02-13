using Ardalis.GuardClauses;
using Catering.Domain.Exceptions;

namespace Catering.Domain.Entities.OrderAggregate;

public class Order
{
    public long Id { get; set; }
    public string CustomerId { get; set; }
    public DateTime ExpectedOn { get; set; }
    public DateTime CreatedOn { get; set; }
    public OrderStatus Status { get; private set; }
    public HomeDeliveryInfo HomeDeliveryInfo { get; private set; }
    public Guid MenuId { get; private set; }

    private readonly List<OrderItem> _items = [];
    public IReadOnlyList<OrderItem> Items => _items.AsReadOnly();

    protected Order() { }

    public Order(
        IEnumerable<OrderItem> items,
        Guid menuId,
        string customerId,
        DateTime expectedOn,
        HomeDeliveryInfo homeDelivery = null)
    {
        Guard.Against.Default(menuId);
        Guard.Against.NullOrWhiteSpace(customerId);
        Guard.Against.Default(expectedOn);

        CreatedOn = DateTime.Now;
        Status = OrderStatus.Subbmited;

        MenuId = menuId;
        AddItems(items);

        CustomerId = customerId;
        ExpectedOn = expectedOn;

        HomeDeliveryInfo = homeDelivery;
    }

    public bool IsForHomeDelivery => HomeDeliveryInfo != default;

    public void ConfirmOrder()
    {
        if (Status != OrderStatus.Subbmited)
            throw new WrongOrderStatusException(nameof(ConfirmOrder), Id);

        Status = OrderStatus.Confirmed;
    }

    public void CancelOrder()
    {
        if (Status != OrderStatus.Subbmited)
            throw new WrongOrderStatusException(nameof(ConfirmOrder), Id);

        Status = OrderStatus.Canceled;
    }

    public decimal TotalPrice => _items.Count > 0 ? _items.Sum(i => i.PriceSnapshot * i.Quantity) : 0;

    private void AddItems(IEnumerable<OrderItem> items)
    {
        Guard.Against.NullOrEmpty(items);

        _items.AddRange(items);
    }
}
