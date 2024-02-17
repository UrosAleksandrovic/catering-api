using Catering.Api.Configuration.Authorization;
using Catering.Application.Aggregates.Carts.Abstractions;
using Catering.Application.Aggregates.Carts.Dtos;
using Microsoft.AspNetCore.Mvc;

namespace Catering.Api.Controllers;

[Route("api/carts")]
[AuthorizeClientsEmployee]
public class CartsController(ICartManagementAppService cartsAppService) : ControllerBase
{
    private readonly ICartManagementAppService _cartsAppService = cartsAppService;

    [HttpGet]
    public async Task<IActionResult> GetByCustomerIdAsync()
    {
        return Ok(await _cartsAppService.GetCartByCustomerIdAsync(this.GetUserId()));
    }

    [HttpPost("items")]
    public async Task<IActionResult> PutItemInCartAsync([FromBody] AddItemToCartDto addItemDto)
    {
        await _cartsAppService.AddItemAsync(this.GetUserId(), addItemDto);

        return NoContent();
    }

    [HttpPut("items/{itemId:Guid}/quantity")]
    public async Task<IActionResult> ChangeQuantity(
        [FromRoute] Guid itemId,
        [FromBody] CartItemQuantityDto newQuantity)
    {
        await _cartsAppService.ChangeQuantity(this.GetUserId(), itemId, newQuantity.Quantity);

        return NoContent();
    }

    [HttpPut("items/{itemId:Guid}/note")]
    public async Task<IActionResult> ChangeNoteAsync(
        [FromRoute] Guid itemId,
        [FromBody] CartItemNoteDto newNote)
    {
        await _cartsAppService.AddOrEditItemNoteAsync(this.GetUserId(), itemId, newNote.Note);

        return NoContent();
    }
}
