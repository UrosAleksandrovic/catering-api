﻿using Catering.Api.Configuration.Authorization;
using Catering.Api.Extensions;
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
        => this.FromResult(await _cartsAppService.GetCartByCustomerIdAsync(this.GetUserId()));

    [HttpPost("items")]
    public async Task<IActionResult> PutItemInCartAsync([FromBody] AddItemToCartDto addItemDto)
        => this.FromResult(await _cartsAppService.AddItemAsync(this.GetUserId(), addItemDto));

    [HttpPut("items/{itemId:Guid}/quantity")]
    public async Task<IActionResult> ChangeQuantity(
        [FromRoute] Guid itemId,
        [FromBody] CartItemQuantityDto newQuantity)
        => this.FromResult(await _cartsAppService.ChangeQuantity(this.GetUserId(), itemId, newQuantity.Quantity));

    [HttpPut("items/{itemId:Guid}/note")]
    public async Task<IActionResult> ChangeNoteAsync(
        [FromRoute] Guid itemId,
        [FromBody] CartItemNoteDto newNote)
        => this.FromResult(await _cartsAppService.AddOrEditItemNoteAsync(this.GetUserId(), itemId, newNote.Note));
}
