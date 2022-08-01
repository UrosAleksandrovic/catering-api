using Catering.Api.Configuration.Authorization;
using Catering.Application.Aggregates.Identites;
using Catering.Application.Aggregates.Identites.Abstractions;
using Catering.Application.Aggregates.Identites.Dtos;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace Catering.Api.Controllers;

[Route("/api/companyEmployees")]
public class CompanyEmployeesController : ControllerBase
{
    private readonly ICustomerManagementAppService _customerAppService;

    public CompanyEmployeesController(ICustomerManagementAppService customerAppService)
    {
        _customerAppService = customerAppService;
    }

    [HttpPost]
    [AuthorizeCompanyAdmins]
    public async Task<IActionResult> RegisterAsync(CreateCustomerDto createRequest)
    {
        var requestorId = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier)?.Value;
        await _customerAppService.CreateCompanyCustomerAsync(createRequest, requestorId);

        return NoContent();
    }

    [HttpGet("budget")]
    [AuthorizeCompanyEmployee]
    public async Task<IActionResult> GetBudgetInfoAsync()
    {
        var requestorId = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier)?.Value;
        var budget = await _customerAppService.GetCustomerBudgetInfoAsync(requestorId);

        if (budget != null)
            return Ok(budget);

        return NotFound();
    }

    [HttpGet]
    [AuthorizeCompanyAdmins]
    public async Task<IActionResult> GetCompanyEmployees([FromQuery] CustomersFilter filter)
    {
        var result = await _customerAppService.GetFilteredAsync(filter);

        return Ok(result);
    }
}
