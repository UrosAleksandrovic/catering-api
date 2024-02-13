using MediatR;

namespace Catering.Application.Scheduling.BudgetReset.Requests;

public class ResetCustomerBudgets : IRequest
{
    public decimal NewBudget { get; set; }
}
