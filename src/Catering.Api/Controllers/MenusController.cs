using Catering.Api.Configuration.Authorization;
using Catering.Api.Extensions;
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
        => this.CreatedAtRouteFromResult(await _menuAppService.CreateAsync(createRequest), GetNameRoute);

    const string GetNameRoute = "GetMenuById";
    [HttpGet("{id:Guid}", Name = GetNameRoute)]
    [Authorize]
    public async Task<IActionResult> GetMenuByIdAsync(Guid id)
        => this.FromResult(await _menuAppService.GetByIdAsync(id, this.GetUserId()));

    [HttpGet]
    [AuthorizeClientsEmployee]
    public async Task<IActionResult> GetPageAsync([FromQuery] MenusFilter filter)
        => this.FromResult(await _publisher.Send(new GetFilteredMenusQuery(filter)));

    [HttpPut("{id:Guid}")]
    [AuthorizeClientsAdmins]
    public async Task<IActionResult> UpdateMenu([FromRoute] Guid id, [FromBody] UpdateMenuDto updateRequest)
        => this.FromResult(await _menuAppService.UpdateAsync(id, updateRequest));

    [HttpDelete("{id:Guid}")]
    [AuthorizeClientsAdmins]
    public async Task<IActionResult> DeleteMenu([FromRoute] Guid id)
        => this.FromResult(await _menuAppService.DeleteAsync(id));

    [HttpGet("contacts")]
    public async Task<IActionResult> GetRestaurantContacts([FromQuery] MenusFilter filter)
        => this.FromResult(await _publisher.Send(new GetFilteredMenuContactsQuery(filter)));
}
