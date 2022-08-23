using Catering.Api.Configuration.Authorization;
using Catering.Application.Aggregates.Identites.Abstractions;
using Catering.Application.Aggregates.Identites.Dtos;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace Catering.Api.Controllers;

[Route("api/restourantEmployees")]
public class RestourantEmployeesController : ControllerBase
{
    private readonly ICateringIdentititesManagementAppService _service;

    public RestourantEmployeesController(ICateringIdentititesManagementAppService service)
    {
        _service = service;
    }

    [HttpPost]
    [AuthorizeClientsAdmins]
    public async Task<IActionResult> RegisterAsync([FromBody] CreateRestourantDto createRequest)
    {
        await _service.CreateRestourantAsync(createRequest);

        return NoContent();
    }
}
