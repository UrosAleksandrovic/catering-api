using Catering.Application.Aggregates.Expenses.Notifications;
using Catering.Application.Aggregates.Identities.Abstractions;
using MediatR;

namespace Catering.Application.Aggregates.Expenses.Handlers
{
    internal class ExpenseUpdatedHandler : INotificationHandler<ExpenseUpdated>
    {
        private readonly ICustomerRepository _customerRepository;

        public ExpenseUpdatedHandler(ICustomerRepository customerRepository)
        {
            _customerRepository = customerRepository;
        }

        public async Task Handle(ExpenseUpdated notification, CancellationToken cancellationToken)
        {
            var customer = await _customerRepository.GetByIdAsync(notification.CustomerId);

            customer.RevertPayment(notification.PreviousPrice);
            customer.ProcessExpense(notification.NewPrice);

            await _customerRepository.UpdateAsync(customer);
        }
    }
}
