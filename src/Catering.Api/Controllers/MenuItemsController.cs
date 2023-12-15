using Catering.Api.Configuration.Authorization;
using Catering.Application.Aggregates.Items;
using Catering.Application.Aggregates.Items.Abstractions;
using Catering.Application.Aggregates.Items.Dtos;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace Catering.Api.Controllers;

[Route("/api/menus")]
public class MenuItemsController : ControllerBase
{
    private readonly IItemManagementAppService _itemsAppService;

    public MenuItemsController(IItemManagementAppService itemsAppService)
    {
        _itemsAppService = itemsAppService;
    }

    [HttpPost("{menuId}/items")]
    [AuthorizeClientsAdmins]
    public async Task<IActionResult> CreateAsync([FromRoute] Guid menuId,[FromBody] CreateItemDto createRequest)
    {
        var createdId = await _itemsAppService.CreateItemAsync(menuId, createRequest);

        return CreatedAtRoute(GetNameRoute, new { id = createdId }, new { id = createdId });
    }


    private const string GetNameRoute = "GetItemById";
    [HttpGet("{menuId}/items/{id}", Name = GetNameRoute)]
    [Authorize]
    public async Task<IActionResult> GetByIdAsync([FromRoute] Guid menuId, [FromRoute] Guid id)
    {
        var userId = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier)?.Value;
        var item = await _itemsAppService.GetItemByIdAsync(menuId, id, userId);

        if (item == default)
            return NotFound();

        return Ok(item);
    }

    [HttpDelete("{menuId}/items/{id}")]
    [AuthorizeClientsAdmins]
    public async Task<IActionResult> DeleteAsync([FromRoute] Guid menuId, [FromRoute] Guid id)
    {
        await _itemsAppService.DeleteItemAsync(menuId, id);

        return NoContent();
    }

    [HttpPut("{menuId}/items/{id}")]
    [AuthorizeClientsAdmins]
    public async Task<IActionResult> UpdateAsync(
        [FromRoute] Guid menuId,
        [FromRoute] Guid id,
        [FromBody] UpdateItemDto updateRequest)
    {
        await _itemsAppService.UpdateItemAsync(menuId, id, updateRequest);

        return NoContent();
    }

    [HttpPut("{menuId}/items/{id}/{rating}")]
    [AuthorizeClientsEmployee]
    public async Task<IActionResult> RateItemAsync(
        [FromRoute] Guid menuId,
        [FromRoute] Guid id,
        [FromRoute] short rating)
    {
        var userId = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier)?.Value;
        await _itemsAppService.RateItemAsync(menuId, id, userId, rating);

        return NoContent();
    }

    [HttpGet("{menuId}/items/{id}/rating")]
    [AuthorizeClientsEmployee]
    public async Task<IActionResult> GetItemRatingAsync(
        [FromRoute] Guid menuId,
        [FromRoute] Guid id)
    {
        var userId = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier)?.Value;
        var result = await _itemsAppService.GetCustomerRatingForItemAsync(menuId, id, userId);

        return Ok(result);
    }

    [HttpGet("items")]
    [Authorize]
    public async Task<IActionResult> GetPageAsync([FromQuery] ItemsFilter filter)
    {
        var userId = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier)?.Value;

        var result = await _itemsAppService.GetFilteredAsync(filter, userId);

        return Ok(result);
    }

    [HttpGet("{menuId}/topOrdered")]
    [Authorize]
    public async Task<IActionResult> GetTopOrdered([FromRoute] Guid menuId, [FromQuery] int top = 10)
    {
        var result = await _itemsAppService.GetMostOrderedFromTheMenuAsync(top, menuId);

        return Ok(new { Items = result });
    }
}
