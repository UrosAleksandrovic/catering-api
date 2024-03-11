using Catering.Api.Configuration.Authorization;
using Catering.Application.Scheduling.BudgetReset;
using Microsoft.AspNetCore.Mvc;

namespace Catering.Api.Controllers;

[Route("api/jobs")]
[AuthorizeClientsAdmins]
public class ManualJobController(IBudgetResetJob budgetResetJob) : ControllerBase
{
    private readonly IBudgetResetJob _budgetResetJob = budgetResetJob;

    [HttpPost("reset-budgets")]
    public async Task<IActionResult> ResetBudgets()
    {
        var result = await _budgetResetJob.ExecuteAsync();

        return Ok(new { IsSuccessfull = result });
    }
}
