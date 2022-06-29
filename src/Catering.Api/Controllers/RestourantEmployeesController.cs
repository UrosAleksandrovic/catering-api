using Catering.Api.Configuration.Authorization;
using Catering.Application.Aggregates.Identites.Abstractions;
using Catering.Application.Aggregates.Identites.Dtos;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace Catering.Api.Controllers;

[Route("api/restourantEmployees")]
public class RestourantEmployeesController : ControllerBase
{
    private readonly IExternalIdentitiesManagementAppService _service;

    public RestourantEmployeesController(IExternalIdentitiesManagementAppService service)
    {
        _service = service;
    }

    [HttpPost]
    [AuthorizeCompanyAdmins]
    public async Task<IActionResult> RegisterAsync([FromBody] CreateRestourantDto createRequest)
    {
        var requestorId = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier)?.Value;
        await _service.CreateRestourantIdentityAsync(createRequest);

        return NoContent();
    }
}
