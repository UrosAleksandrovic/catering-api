using MediatR;

namespace Catering.Application.Aggregates.Expenses.Notifications;

public record ExpenseUpdated(string CustomerId, decimal PreviousPrice, decimal NewPrice) : INotification;
