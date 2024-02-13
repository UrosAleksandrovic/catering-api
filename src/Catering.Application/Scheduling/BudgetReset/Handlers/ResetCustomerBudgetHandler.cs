using Catering.Application.Aggregates.Identities.Abstractions;
using Catering.Application.Scheduling.BudgetReset.Requests;
using MediatR;

namespace Catering.Application.Scheduling.BudgetReset.Handlers;

internal class ResetCustomerBudgetHandler : IRequestHandler<ResetCustomerBudgets>
{
    private readonly ICustomerRepository _customerRepository;

    public ResetCustomerBudgetHandler(ICustomerRepository customerRepository)
    {
        _customerRepository = customerRepository;
    }

    public async Task Handle(ResetCustomerBudgets request, CancellationToken cancellationToken)
    {
        await _customerRepository.ResetBudgetAsync(request.NewBudget);
    }
}
