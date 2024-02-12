using MediatR;

namespace Catering.Application.Aggregates.Expenses.Notifications;

public class ExpenseCreated : INotification
{
    public string CustomerId { get; set; }
    public decimal Price { get; set; }
}
