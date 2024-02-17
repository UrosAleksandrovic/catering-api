using Catering.Api.Configuration.Authorization;
using Catering.Application.Aggregates.Menus;
using Catering.Application.Aggregates.Menus.Abstractions;
using Catering.Application.Aggregates.Menus.Dtos;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Catering.Api.Controllers;

[Route("/api/menus")]
public class MenusController(IMenuManagementAppService menuAppService) : ControllerBase
{
    private readonly IMenuManagementAppService _menuAppService = menuAppService;

    [HttpPost]
    [AuthorizeClientsAdmins]
    public async Task<IActionResult> CreateMenuAsync([FromBody] CreateMenuDto createRequest)
    {
        var createdId = await _menuAppService.CreateAsync(createRequest);

        return CreatedAtRoute(GetNameRoute, new { id = createdId }, new { id = createdId });
    }

    const string GetNameRoute = "GetMenuById";
    [HttpGet("{id:Guid}", Name = GetNameRoute)]
    [Authorize]
    public async Task<IActionResult> GetMenuByIdAsync(Guid id)
    {
        var menu = await _menuAppService.GetByIdAsync(id, this.GetUserId());

        if (menu == null)
            return NotFound();

        return Ok(menu);
    }

    [HttpGet]
    [AuthorizeClientsEmployee]
    public async Task<IActionResult> GetPageAsync([FromQuery] MenusFilter filter)
    {
        var result = await _menuAppService.GetFilteredAsync(filter);

        return Ok(result);
    } 

    [HttpPut("{id:Guid}")]
    [AuthorizeClientsAdmins]
    public async Task<IActionResult> UpdateMenu([FromRoute] Guid id, [FromBody] UpdateMenuDto updateRequest)
    {
        await _menuAppService.UpdateAsync(id, updateRequest);

        return NoContent();
    }

    [HttpDelete("{id:Guid}")]
    [AuthorizeClientsAdmins]
    public async Task<IActionResult> DeleteMenu([FromRoute] Guid id)
    {
        await _menuAppService.DeleteAsync(id);

        return NoContent();
    }

    [HttpGet("contacts")]
    public async Task<IActionResult> GetRestaurantContacts([FromQuery] MenusFilter filter)
    {
        return Ok(await _menuAppService.GetRestaurantContactsAsync(filter));
    }
}
