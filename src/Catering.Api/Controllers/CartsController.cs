using Catering.Api.Configuration.Authorization;
using Catering.Application.Aggregates.Carts.Abstractions;
using Catering.Application.Aggregates.Carts.Dtos;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace Catering.Api.Controllers;

[Route("api/carts")]
[AuthorizeClientsEmployee]
public class CartsController : ControllerBase
{
    private readonly ICartManagementAppService _cartsAppService;

    public CartsController(ICartManagementAppService cartsAppService)
    {
        _cartsAppService = cartsAppService;
    }

    [HttpGet]
    public async Task<IActionResult> GetByCustomerIdAsync()
    {
        var customerId = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier)?.Value;

        return Ok(await _cartsAppService.GetCartByCustomerIdAsync(customerId));
    }

    [HttpPut("items")]
    public async Task<IActionResult> PutItemInCartAsync([FromBody] AddItemToCartDto addItemDto)
    {
        var customerId = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier)?.Value;
        await _cartsAppService.AddItemAsync(customerId, addItemDto);

        return NoContent();
    }

    [HttpPut("items/{itemId}/increment")]
    public async Task<IActionResult> IncrementAsync([FromRoute] Guid itemId, [FromQuery] int quantity = 1)
    {
        var customerId = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier)?.Value;
        await _cartsAppService.IncrementItemAsync(customerId, itemId, quantity);

        return NoContent();
    }

    [HttpPut("items/{itemId}/decrement")]
    public async Task<IActionResult> DecrementAsync([FromRoute] Guid itemId, [FromQuery] int quantity = 1)
    {
        var customerId = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier)?.Value;
        await _cartsAppService.DecrementItemAsync(customerId, itemId, quantity);

        return NoContent();
    }

    [HttpPut("items/{itemId}/note")]
    public async Task<IActionResult> ChangeNoteAsync([FromRoute] Guid itemId, [FromBody] CartItemNoteDto newNote)
    {
        var customerId = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier)?.Value;
        await _cartsAppService.AddOrEditItemNoteAsync(customerId, itemId, newNote.Note);

        return NoContent();
    }
}
