using MediatR;

namespace Catering.Application.Aggregates.Expenses.Notifications;

public record ExpenseCreated(string CustomerId, decimal Price) : INotification;
