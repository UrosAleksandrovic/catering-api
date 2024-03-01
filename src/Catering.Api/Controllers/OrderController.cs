using Catering.Api.Configuration.Authorization;
using Catering.Api.Extensions;
using Catering.Application.Aggregates.Orders;
using Catering.Application.Aggregates.Orders.Abstractions;
using Catering.Application.Aggregates.Orders.Dtos;
using Catering.Application.Aggregates.Orders.Queries;
using Catering.Application.Results;
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
        var createdResult = await _ordersService.PlaceOrderAsync(this.GetUserId(), createOrder);

        return this.CreatedAtRouteFromResult(createdResult, GetNameRoute);
    }

    [HttpPut("{id:long}/status")]
    [AuthorizeRestaurantEmployee]
    public async Task<IActionResult> ChangeStatusAsync(
        [FromRoute] long id,
        [FromBody] ChangeOrderStatusDto newStatus)
    {
        var result = newStatus.NewStatus switch
        {
            OrderStatus.Canceled => await _ordersService.CancelAsync(id),
            OrderStatus.Confirmed => await _ordersService.ConfirmAsync(id),
            _ => Result.Error(ErrorType.Unknown)
        };

        return this.FromResult(result);
    }

    private const string GetNameRoute = "GetOrderById";
    [HttpGet("{id:long}", Name = GetNameRoute)]
    [Authorize]
    public async Task<IActionResult> GetOrderByIdAsync([FromRoute] long id)
        => this.FromResult(await _publisher.Send(new GetOrderByIdQuery(id, this.GetUserId())));

    [HttpGet]
    [Authorize]
    public async Task<IActionResult> GetFilteredOrdersAsync([FromQuery] OrdersFilter filter)
        => this.FromResult(await _publisher.Send(new GetFilteredOrdersQuery(filter, this.GetUserId())));
}
