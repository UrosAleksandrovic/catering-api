using Catering.Domain.Aggregates.Expense;

namespace Catering.Domain.Builders;

public class ExpenseBuilder : IBuilder<Expense>
{
    private string _customerId;
    private Guid _menuId;
    private DateTimeOffset _deliveredOn;
    private decimal _price;
    private string _note;

    public Expense Build()
    {
        var expense = new Expense(_menuId, _customerId, _deliveredOn, _price);

        if (!string.IsNullOrWhiteSpace(_note))
            expense.AddNote(_note);

        return expense;
    }

    public ExpenseBuilder HasMenuAndCustomer(Guid menuId, string customerId)
    {
        _customerId = customerId;
        _menuId = menuId;

        return this;
    }

    public ExpenseBuilder HasPriceAndDate(decimal price, DateTimeOffset deliveredOn)
    {
        _deliveredOn = deliveredOn;
        _price = price;

        return this;
    }

    public ExpenseBuilder HasNote(string note)
    {
        _note = note;

        return this;
    }

    public void Reset()
    {
        _customerId = _note = default;
        _menuId = default;
        _deliveredOn = default;
        _price = default;
    }
}
