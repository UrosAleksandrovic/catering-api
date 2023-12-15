using Catering.Api.Configuration.Authorization;
using Catering.Application.Aggregates.Orders;
using Catering.Application.Aggregates.Orders.Abstractions;
using Catering.Application.Aggregates.Orders.Dtos;
using Catering.Domain.Entities.IdentityAggregate;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace Catering.Api.Controllers;

[Route("/api/orders")]
public class OrdersController : ControllerBase
{
    private readonly IOrderManagementAppService _ordersService;

    public OrdersController(IOrderManagementAppService ordersService)
    {
        _ordersService = ordersService;
    }

    [HttpPost]
    [AuthorizeClientsEmployee]
    public async Task<IActionResult> MakeOrderAsync([FromBody] CreateOrderDto createOrder)
    {
        var customerId = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier)?.Value;

        var orderId = await _ordersService.PlaceOrderAsync(customerId, createOrder);

        return CreatedAtRoute(GetNameRoute, new { id = orderId }, new { id = orderId });
    }

    [HttpPut("{orderId}/cancel")]
    [CateringAuthorization(IdentityRole.Administrator | IdentityRole.Super,
        IdentityRole.Administrator | IdentityRole.Client | IdentityRole.Employee,
        IdentityRole.Restaurant | IdentityRole.Employee)]
    public async Task<IActionResult> CancelOrderAsync([FromRoute] long orderId)
    {
        await _ordersService.CancelAsync(orderId);

        return NoContent();
    }

    [HttpPut("{orderId}/confirm")]
    [CateringAuthorization(IdentityRole.Administrator | IdentityRole.Super,
        IdentityRole.Administrator | IdentityRole.Client | IdentityRole.Employee,
        IdentityRole.Restaurant | IdentityRole.Employee)]
    public async Task<IActionResult> ConfirmOrderAsync([FromRoute] long orderId)
    {
        await _ordersService.ConfirmAsync(orderId);

        return NoContent();
    }

    private const string GetNameRoute = "GetOrderById";
    [HttpGet("{id}", Name = GetNameRoute)]
    [Authorize]
    public async Task<IActionResult> GetOrderByIdAsync([FromRoute] long id)
    {
        var requesterId = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier)?.Value;
        var order = await _ordersService.GetByIdAsync(id, requesterId);

        if (order == default)
            return NotFound();

        return Ok(order);
    }

    [HttpGet]
    [Authorize]
    public async Task<IActionResult> GetFilteredOrdersAsync([FromQuery] OrdersFilter filter)
    {
        var requesterId = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier)?.Value;
        var orders = await _ordersService.GetFilteredAsync(filter, requesterId);

        return Ok(orders);
    }
}
