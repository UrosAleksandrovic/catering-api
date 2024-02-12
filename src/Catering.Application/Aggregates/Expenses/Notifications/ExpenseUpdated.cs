using MediatR;

namespace Catering.Application.Aggregates.Expenses.Notifications;

public class ExpenseUpdated : INotification
{
    public string CustomerId { get; set; }
    public decimal PreviousPrice { get; set; }
    public decimal NewPrice { get; set; }
}
