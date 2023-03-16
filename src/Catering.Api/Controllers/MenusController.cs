using Catering.Api.Configuration.Authorization;
using Catering.Application.Aggregates.Menus;
using Catering.Application.Aggregates.Menus.Abstractions;
using Catering.Application.Aggregates.Menus.Dtos;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace Catering.Api.Controllers;

[Route("/api/menus")]
public class MenusController : ControllerBase
{
    private readonly IMenuManagementAppService _menuAppService;

    public MenusController(IMenuManagementAppService menuAppService)
    {
        _menuAppService = menuAppService;
    }

    [HttpPost]
    [AuthorizeClientsAdmins]
    public async Task<IActionResult> CreateMenuAsync([FromBody] CreateMenuDto createRequest)
    {
        var createdId = await _menuAppService.CreateAsync(createRequest);

        return CreatedAtRoute(GetNameRoute, new { id = createdId }, new { id = createdId });
    }

    const string GetNameRoute = "GetMenuById";
    [HttpGet("{id}", Name = GetNameRoute)]
    [Authorize]
    public async Task<IActionResult> GetMenuByIdAsync(Guid id)
    {
        var requestorId = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier)?.Value;
        var menu = await _menuAppService.GetByIdAsync(id, requestorId);

        if (menu == null)
            return NotFound();

        return Ok(menu);
    }

    [HttpGet("page")]
    [AuthorizeClientsEmployee]
    public async Task<IActionResult> GetPageAsync([FromQuery] MenusFilter filter)
    {
        var result = await _menuAppService.GetFilteredAsync(filter);

        return Ok(result);
    } 

    [HttpPut("{id}")]
    [AuthorizeClientsAdmins]
    public async Task<IActionResult> UpdateMenu([FromRoute] Guid id, [FromBody] UpdateMenuDto updateRequest)
    {
        await _menuAppService.UpdateAsync(id, updateRequest);

        return NoContent();
    }

    [HttpDelete("{id}")]
    [AuthorizeClientsAdmins]
    public async Task<IActionResult> DeleteMenu([FromRoute] Guid id)
    {
        await _menuAppService.DeleteAsync(id);

        return NoContent();
    }
}
