using Catering.Api.Configuration.Authorization;
using Catering.Application.Aggregates.Identities.Abstractions;
using Catering.Application.Aggregates.Identities.Dtos;
using Microsoft.AspNetCore.Mvc;

namespace Catering.Api.Controllers;

[Route("api/restaurantEmployees")]
public class RestaurantEmployeesController : ControllerBase
{
    private readonly ICateringIdentitiesManagementAppService _service;

    public RestaurantEmployeesController(ICateringIdentitiesManagementAppService service)
    {
        _service = service;
    }

    [HttpPost]
    [AuthorizeClientsAdmins]
    public async Task<IActionResult> RegisterAsync([FromBody] CreateRestaurantDto createRequest)
    {
        await _service.CreateRestaurantAsync(createRequest);

        return NoContent();
    }
}
