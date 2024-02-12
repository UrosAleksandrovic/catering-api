using MediatR;

namespace Catering.Application.Scheduling.BudgetReset.Notifications
{
    public class BudgetResetFailed : INotification
    {
        public int Month { get; set; }
    }
}
