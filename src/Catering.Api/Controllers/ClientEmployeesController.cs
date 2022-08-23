using Catering.Api.Configuration.Authorization;
using Catering.Application.Aggregates.Identites;
using Catering.Application.Aggregates.Identites.Abstractions;
using Catering.Application.Aggregates.Identites.Dtos;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace Catering.Api.Controllers;

[Route("/api/clientEmployee")]
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
        var requestorId = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier)?.Value;
        await _customerAppService.CreateClientsCustomerAsync(createRequest, requestorId);

        return NoContent();
    }

    [HttpGet("budget")]
    [AuthorizeClientsEmployee]
    public async Task<IActionResult> GetBudgetInfoAsync()
    {
        var requestorId = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier)?.Value;
        var budget = await _customerAppService.GetCustomerBudgetInfoAsync(requestorId);

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
        var requestorId = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier)?.Value;
        var customerInfoDto = await _customerAppService.GetCustomerInfoAsync(requestorId);

        if (customerInfoDto != null)
            return Ok(customerInfoDto);

        return NotFound();
    }
}
