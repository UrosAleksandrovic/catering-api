using Catering.Api.Configuration.Authorization;
using Catering.Application.Scheduling.BudgetReset;
using Microsoft.AspNetCore.Mvc;

namespace Catering.Api.Controllers
{
    [Route("api/jobs/")]
    [AuthorizeClientsAdmins]
    public class ManualJobController : ControllerBase
    {
        private readonly IBudgetResetJob _budgetResetJob;

        public ManualJobController(IBudgetResetJob budgetResetJob)
        {
            _budgetResetJob = budgetResetJob;
        }

        [HttpPost("reset-budgets")]
        public async Task<IActionResult> ResetBudgets()
        {
            var result = await _budgetResetJob.ExecuteAsync();

            return result ? Ok() : StatusCode(500);
        }
    }
}
