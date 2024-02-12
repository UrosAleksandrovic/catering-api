using Catering.Application.Scheduling.BudgetReset.Notifications;
using MediatR;

namespace Catering.Application.Scheduling.BudgetReset.Handlers
{
    internal class BudgetResetFailedHandler : INotificationHandler<BudgetResetFailed>
    {
        public Task Handle(BudgetResetFailed notification, CancellationToken cancellationToken)
        {
            return Task.CompletedTask;
        }
    }
}
