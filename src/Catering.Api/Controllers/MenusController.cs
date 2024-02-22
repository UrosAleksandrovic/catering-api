using Catering.Api.Configuration.Authorization;
using Catering.Application.Aggregates.Menus;
using Catering.Application.Aggregates.Menus.Abstractions;
using Catering.Application.Aggregates.Menus.Dtos;
using Catering.Application.Aggregates.Menus.Queries;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Catering.Api.Controllers;

[Route("/api/menus")]
public class MenusController(
    IMenusManagementAppService menuAppService,
    IMediator publisher) : ControllerBase
{
    private readonly IMenusManagementAppService _menuAppService = menuAppService;
    private readonly IMediator _publisher = publisher;

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
        return Ok(await _publisher.Send(new GetFilteredMenusQuery(filter)));
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
        return Ok(await _publisher.Send(new GetFilteredMenuContactsQuery(filter)));
    }
}
