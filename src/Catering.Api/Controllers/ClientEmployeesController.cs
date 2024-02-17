using Catering.Api.Configuration.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using Catering.Application.Aggregates.Identities;
using Catering.Application.Aggregates.Identities.Abstractions;
using Catering.Application.Aggregates.Identities.Dtos;

namespace Catering.Api.Controllers;

[Route("/api/clientEmployees")]
public class ClientEmployeesController(ICustomerManagementAppService customerAppService) : ControllerBase
{
    private readonly ICustomerManagementAppService _customerAppService = customerAppService;

    [HttpPost]
    [AuthorizeClientsAdmins]
    public async Task<IActionResult> RegisterAsync([FromBody] CreateCustomerDto createRequest)
    {
        await _customerAppService.CreateClientsCustomerAsync(createRequest, this.GetUserId());

        return NoContent();
    }

    [HttpGet("profiles/budget")]
    [AuthorizeClientsEmployee]
    public async Task<IActionResult> GetBudgetInfoAsync()
    {
        var budget = await _customerAppService.GetCustomerBudgetInfoAsync(this.GetUserId());

        if (budget != null)
            return Ok(budget);

        return NotFound();
    }

    [HttpGet("profiles")]
    [AuthorizeClientsEmployee]
    public async Task<IActionResult> GetProfileAsync()
    {
        var customerInfoDto = await _customerAppService.GetCustomerInfoAsync(this.GetUserId());

        if (customerInfoDto != null)
            return Ok(customerInfoDto);

        return NotFound();
    }

    [HttpGet("/api/internal/clientEmployees")]
    [AuthorizeClientsAdmins]
    public async Task<IActionResult> GetInternalClientsEmployeesAsync([FromQuery] CustomersFilter filter)
    {
        var result = await _customerAppService.GetFilteredInternalAsync(filter);

        return Ok(result);
    }

    [HttpGet("/api/external/clientEmployees")]
    [AuthorizeClientsAdmins]
    public async Task<IActionResult> GetExternalClientEmployeesAsync([FromQuery] CustomersFilter filter)
    {
        var result = await _customerAppService.GetFilteredExternalAsync(filter);

        return Ok(result);
    }
}
