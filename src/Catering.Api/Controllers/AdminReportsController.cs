using Catering.Application.Aggregates.Identities.Abstractions;
using Microsoft.AspNetCore.Mvc;

namespace Catering.Api.Controllers;

[Route("/api/admin/reports")]
public class AdminReportsController : ControllerBase
{

    private readonly ICustomerReportsAppService _customerReportsAppService;

    public AdminReportsController(ICustomerReportsAppService customerReportsAppService)
    {
        _customerReportsAppService = customerReportsAppService;
    }

    [HttpGet("monthly-spendings")]
    public async Task<IActionResult> GetMonthlySendings([FromQuery] int month, [FromQuery] int year)
    {
        var monthlySpending = await _customerReportsAppService.GetMonthlySendingAsync(month, year);

        return Ok(new { data = monthlySpending });
    }
}
