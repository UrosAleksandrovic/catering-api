using Catering.Api.Configuration.Authorization;
using Catering.Application.Aggregates.Identities.Abstractions;
using Microsoft.AspNetCore.Mvc;

namespace Catering.Api.Controllers;

[Route("/api/admin/reports")]
[AuthorizeClientsAdmins]
public class AdminReportsController(ICustomerReportsAppService customerReportsAppService) : ControllerBase
{
    private readonly ICustomerReportsAppService _customerReportsAppService = customerReportsAppService;

    [HttpGet("monthly-spendings")]
    public async Task<IActionResult> GetMonthlySendings([FromQuery] int? month = null, [FromQuery] int? year = null)
    {
        var monthlySpending = await _customerReportsAppService.GetMonthlySendingAsync(month, year);

        return Ok(new { data = monthlySpending });
    }
}
