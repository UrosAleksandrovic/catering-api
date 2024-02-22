using Catering.Api.Configuration.Authorization;
using Catering.Application.Aggregates.Orders;
using Catering.Application.Aggregates.Orders.Abstractions;
using Catering.Application.Aggregates.Orders.Dtos;
using Catering.Application.Aggregates.Orders.Queries;
using Catering.Domain.Aggregates.Identity;
using Catering.Domain.Aggregates.Order;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Catering.Api.Controllers;

[Route("/api/orders")]
public class OrdersController(IOrderManagementAppService ordersService, IMediator publisher) : ControllerBase
{
    private readonly IOrderManagementAppService _ordersService = ordersService;
    private readonly IMediator _publisher = publisher;

    [HttpPost]
    [AuthorizeClientsEmployee]
    public async Task<IActionResult> MakeOrderAsync([FromBody] CreateOrderDto createOrder)
    {
        var orderId = await _ordersService.PlaceOrderAsync(this.GetUserId(), createOrder);

        return CreatedAtRoute(GetNameRoute, new { id = orderId }, new { id = orderId });
    }

    [HttpPut("{id:long}/status")]
    [CateringAuthorization(IdentityRole.Administrator | IdentityRole.Super,
        IdentityRole.Administrator | IdentityRole.Client | IdentityRole.Employee,
        IdentityRole.Restaurant | IdentityRole.Employee)]
    public async Task<IActionResult> ChangeStatusAsync([FromRoute] long id, [FromBody] ChangeOrderStatusDto newStatus)
    {
        switch (newStatus.NewStatus)
        {
            case OrderStatus.Canceled:
                await _ordersService.CancelAsync(id);
                break;
            case OrderStatus.Confirmed:
                await _ordersService.ConfirmAsync(id);
                break;
            default:
                return BadRequest();
        }

        return NoContent();
    }

    private const string GetNameRoute = "GetOrderById";
    [HttpGet("{id:long}", Name = GetNameRoute)]
    [Authorize]
    public async Task<IActionResult> GetOrderByIdAsync([FromRoute] long id)
    {
        var order = await _publisher.Send(new GetOrderByIdQuery(id, this.GetUserId()));

        if (order == default)
            return NotFound();

        return Ok(order);
    }

    [HttpGet]
    [Authorize]
    public async Task<IActionResult> GetFilteredOrdersAsync([FromQuery] OrdersFilter filter)
    {
        var orders = await _publisher.Send(new GetFilteredOrdersQuery(filter, this.GetUserId()));

        return Ok(orders);
    }
}
