using Catering.Api.Configuration.Authorization;
using Catering.Application.Aggregates.Items;
using Catering.Application.Aggregates.Items.Abstractions;
using Catering.Application.Aggregates.Items.Dtos;
using Catering.Application.Aggregates.Items.Queries;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace Catering.Api.Controllers;

[Route("/api/menus")]
public class MenuItemsController(IItemManagementAppService itemsAppService, IMediator publisher) : ControllerBase
{
    private readonly IItemManagementAppService _itemsAppService = itemsAppService;
    private readonly IMediator _publisher = publisher;

    [HttpPost("{menuId:Guid}/items")]
    [AuthorizeClientsAdmins]
    public async Task<IActionResult> CreateAsync([FromRoute] Guid menuId, [FromBody] CreateItemDto createRequest)
    {
        var createdId = await _itemsAppService.CreateItemAsync(menuId, createRequest);

        return CreatedAtRoute(GetNameRoute, new { id = createdId }, new { id = createdId });
    }


    private const string GetNameRoute = "GetItemById";
    [HttpGet("{menuId:Guid}/items/{itemId:Guid}", Name = GetNameRoute)]
    [Authorize]
    public async Task<IActionResult> GetByIdAsync([FromRoute] Guid menuId, [FromRoute] Guid itemId)
    {
        var item = await _itemsAppService.GetItemByIdAsync(menuId, itemId, this.GetUserId());

        if (item == default)
            return NotFound();

        return Ok(item);
    }

    [HttpDelete("{menuId:Guid}/items/{itemId:Guid}")]
    [AuthorizeClientsAdmins]
    public async Task<IActionResult> DeleteAsync([FromRoute] Guid menuId, [FromRoute] Guid itemId)
    {
        await _itemsAppService.DeleteItemAsync(menuId, itemId);

        return NoContent();
    }

    [HttpPut("{menuId:Guid}/items/{itemId:Guid}")]
    [AuthorizeClientsAdmins]
    public async Task<IActionResult> UpdateAsync(
        [FromRoute] Guid menuId,
        [FromRoute] Guid itemId,
        [FromBody] UpdateItemDto updateRequest)
    {
        await _itemsAppService.UpdateItemAsync(menuId, itemId, updateRequest);

        return NoContent();
    }

    [HttpPut("{menuId:Guid}/items/{itemId:Guid}/ratings/{rating}")]
    [AuthorizeClientsEmployee]
    public async Task<IActionResult> RateItemAsync(
        [FromRoute] Guid menuId,
        [FromRoute] Guid itemId,
        [FromRoute] short rating)
    {
        await _itemsAppService.RateItemAsync(menuId, itemId, this.GetUserId(), rating);

        return NoContent();
    }

    [HttpGet("{menuId:Guid}/items/{itemId:Guid}/ratings")]
    [AuthorizeClientsEmployee]
    public async Task<IActionResult> GetItemRatingAsync(
        [FromRoute] Guid menuId,
        [FromRoute] Guid itemId)
    {
        var result = await _itemsAppService.GetCustomerRatingForItemAsync(menuId, itemId, this.GetUserId());

        return Ok(result);
    }

    [HttpGet("items")]
    [Authorize]
    public async Task<IActionResult> GetPageAsync([FromQuery] ItemsFilter filter)
    {
        return Ok(await _publisher.Send(new GetFilteredItemsQuery(filter, this.GetUserId())));
    }

    [HttpGet("{menuId:Guid}/top-orders")]
    [Authorize]
    public async Task<IActionResult> GetTopOrdered([FromRoute] Guid menuId, [FromQuery] int top = 10)
    {
        return Ok(new { Result = await _publisher.Send(new GetMostOrderedFromTheMenuQuery(menuId, top)) });
    }
}
