using Catering.Api.Configuration.Authorization;
using Catering.Api.Extensions;
using Catering.Application.Aggregates.Items;
using Catering.Application.Aggregates.Items.Abstractions;
using Catering.Application.Aggregates.Items.Dtos;
using Catering.Application.Aggregates.Items.Queries;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Catering.Api.Controllers;

[Route("/api/menus")]
public class MenuItemsController(IItemManagementAppService itemsAppService, IMediator publisher) : ControllerBase
{
    private readonly IItemManagementAppService _itemsAppService = itemsAppService;
    private readonly IMediator _publisher = publisher;

    [HttpPost("{menuId:Guid}/items")]
    [AuthorizeClientsAdmins]
    public async Task<IActionResult> CreateAsync([FromRoute] Guid menuId, [FromBody] CreateItemDto createRequest)
        => this.CreatedAtRouteFromResult(
            await _itemsAppService.CreateItemAsync(menuId, createRequest),
            GET_ITEM_BY_ID_ROUTE);


    private const string GET_ITEM_BY_ID_ROUTE = nameof(GET_ITEM_BY_ID_ROUTE);
    [HttpGet("{menuId:Guid}/items/{itemId:Guid}", Name = GET_ITEM_BY_ID_ROUTE)]
    [Authorize]
    public async Task<IActionResult> GetByIdAsync([FromRoute] Guid menuId, [FromRoute] Guid itemId)
        => this.FromResult(await _itemsAppService.GetItemByIdAsync(menuId, itemId, this.GetUserId()));

    [HttpDelete("{menuId:Guid}/items/{itemId:Guid}")]
    [AuthorizeClientsAdmins]
    public async Task<IActionResult> DeleteAsync([FromRoute] Guid menuId, [FromRoute] Guid itemId)
        => this.FromResult(await _itemsAppService.DeleteItemAsync(menuId, itemId));

    [HttpPut("{menuId:Guid}/items/{itemId:Guid}")]
    [AuthorizeClientsAdmins]
    public async Task<IActionResult> UpdateAsync(
        [FromRoute] Guid menuId,
        [FromRoute] Guid itemId,
        [FromBody] UpdateItemDto updateRequest)
        => this.FromResult(await _itemsAppService.UpdateItemAsync(menuId, itemId, updateRequest));

    [HttpPut("{menuId:Guid}/items/{itemId:Guid}/ratings/{rating}")]
    [AuthorizeClientsEmployee]
    public async Task<IActionResult> RateItemAsync(
        [FromRoute] Guid menuId,
        [FromRoute] Guid itemId,
        [FromRoute] short rating)
        => this.FromResult(await _itemsAppService.RateItemAsync(menuId, itemId, this.GetUserId(), rating));

    [HttpGet("{menuId:Guid}/items/{itemId:Guid}/ratings")]
    [AuthorizeClientsEmployee]
    public async Task<IActionResult> GetItemRatingAsync(
        [FromRoute] Guid menuId,
        [FromRoute] Guid itemId)
        => this.FromResult(await _itemsAppService.GetCustomerRatingForItemAsync(menuId, itemId, this.GetUserId()));

    [HttpGet("{menuId:Guid}/items")]
    [Authorize]
    public async Task<IActionResult> GetPageAsync([FromRoute] Guid menuId, [FromQuery] ItemsFilter filter)
    {
        filter.MenuId = menuId;
        
        return this.FromResult(await _publisher.Send(new GetFilteredItemsQuery(filter, this.GetUserId())));
    }

    [HttpGet("{menuId:Guid}/top-orders")]
    [Authorize]
    public async Task<IActionResult> GetTopOrdered([FromRoute] Guid menuId, [FromQuery] int top = 10)
        => this.FromResult(await _publisher.Send(new GetMostOrderedFromTheMenuQuery(menuId, top)));
}
