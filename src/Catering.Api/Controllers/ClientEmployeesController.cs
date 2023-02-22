using Catering.Api.Configuration.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using Catering.Application.Aggregates.Identities;
using Catering.Application.Aggregates.Identities.Abstractions;
using Catering.Application.Aggregates.Identities.Dtos;

namespace Catering.Api.Controllers;

[Route("/api/clientEmployees")]
public class ClientEmployeesController : ControllerBase
{
    private readonly ICustomerManagementAppService _customerAppService;

    public ClientEmployeesController(ICustomerManagementAppService customerAppService)
    {
        _customerAppService = customerAppService;
    }

    [HttpPost]
    [AuthorizeClientsAdmins]
    public async Task<IActionResult> RegisterAsync(CreateCustomerDto createRequest)
    {
        var requesterId = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier)?.Value;
        await _customerAppService.CreateClientsCustomerAsync(createRequest, requesterId);

        return NoContent();
    }

    [HttpGet("budget")]
    [AuthorizeClientsEmployee]
    public async Task<IActionResult> GetBudgetInfoAsync()
    {
        var requesterId = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier)?.Value;
        var budget = await _customerAppService.GetCustomerBudgetInfoAsync(requesterId);

        if (budget != null)
            return Ok(budget);

        return NotFound();
    }

    [HttpGet]
    [AuthorizeClientsAdmins]
    public async Task<IActionResult> GetClientsEmployeesAsync([FromQuery] CustomersFilter filter)
    {
        var result = await _customerAppService.GetFilteredAsync(filter);

        return Ok(result);
    }

    [HttpGet("profile")]
    [AuthorizeClientsEmployee]
    public async Task<IActionResult> GetProfileAsync()
    {
        var requesterId = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier)?.Value;
        var customerInfoDto = await _customerAppService.GetCustomerInfoAsync(requesterId);

        if (customerInfoDto != null)
            return Ok(customerInfoDto);

        return NotFound();
    }
}
