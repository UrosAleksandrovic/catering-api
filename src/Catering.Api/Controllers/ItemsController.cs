using Catering.Api.Configuration.Authorization;
using Catering.Application.Aggregates.Items;
using Catering.Application.Aggregates.Items.Abstractions;
using Catering.Application.Aggregates.Items.Dtos;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace Catering.Api.Controllers;

[Route("/api/items")]
//TODO: Correct tests
//TODO: Make migrations
public class ItemsController : ControllerBase
{
    private readonly IItemManagementAppService _itemsAppService;

    public ItemsController(IItemManagementAppService itemsAppService)
    {
        _itemsAppService = itemsAppService;
    }

    [HttpPost]
    [AuthorizeClientsAdmins]
    public async Task<IActionResult> CreateAsync([FromBody] CreateItemDto createRequest)
    {
        var createdId = await _itemsAppService.CreateItemAsync(createRequest);

        return CreatedAtRoute(GetNameRoute, new { id = createdId }, new { id = createdId });
    }


    private const string GetNameRoute = "GetItemById";
    [HttpGet("{id}", Name = GetNameRoute)]
    [Authorize]
    public async Task<IActionResult> GetByIdAsync([FromRoute] Guid id)
    {
        var userId = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier)?.Value;
        var item = await _itemsAppService.GetItemByIdAsync(id, userId);

        if (item == default)
            return NotFound();

        return Ok(item);
    }

    [HttpDelete("{id}")]
    [AuthorizeClientsAdmins]
    public async Task<IActionResult> DeleteAsync([FromRoute] Guid id)
    {
        await _itemsAppService.DeleteItemAsync(id);

        return NoContent();
    }

    [HttpPut("{id}")]
    [AuthorizeClientsAdmins]
    public async Task<IActionResult> UpdateAsync([FromRoute] Guid id, [FromBody] UpdateItemDto updateRequest)
    {
        await _itemsAppService.UpdateItemAsync(id, updateRequest);

        return NoContent();
    }

    [HttpPut("{id}/{rating}")]
    [AuthorizeClientsEmployee]
    public async Task<IActionResult> RateItemAsync([FromRoute] Guid id, [FromRoute] short rating)
    {
        var userId = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier)?.Value;
        await _itemsAppService.RateItemAsync(id, userId, rating);

        return NoContent();
    }

    [HttpGet]
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
