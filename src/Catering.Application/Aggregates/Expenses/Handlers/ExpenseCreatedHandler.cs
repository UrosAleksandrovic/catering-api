using Catering.Application.Aggregates.Expenses.Notifications;
using Catering.Application.Aggregates.Identities.Abstractions;
using MediatR;

namespace Catering.Application.Aggregates.Expenses.Handlers;

internal class ExpenseCreatedHandler : INotificationHandler<ExpenseCreated>
{
    private readonly ICustomerRepository _customerRepository;

    public ExpenseCreatedHandler(ICustomerRepository customerRepository)
    {
        _customerRepository = customerRepository;
    }

    public async Task Handle(ExpenseCreated notification, CancellationToken cancellationToken)
    {
        var customer = await _customerRepository.GetByIdAsync(notification.CustomerId);

        customer.ProcessExpense(notification.Price);
        await _customerRepository.UpdateAsync(customer);
    }
}
