using Catering.Api.Configuration.Authorization;
using Catering.Api.Extensions;
using Catering.Application.Aggregates.Identities.Queries;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Catering.Api.Controllers;

[Route("/api/identities/customers")]
public class CustomerIdentitiesController(IMediator publisher) : ControllerBase
{
    private readonly IMediator _publisher = publisher;

    [HttpGet("profile/budget")]
    [AuthorizeClientsEmployee]
    public async Task<IActionResult> GetBudgetInfoAsync()
        => this.FromResult(await _publisher.Send(new GetCustomerBudgetQuery(this.GetUserId())));

    [HttpGet("profile")]
    [AuthorizeClientsEmployee]
    public async Task<IActionResult> GetCustomerProfileAsync()
        => this.FromResult(await _publisher.Send(new GetCustomerInfoQuery(this.GetUserId())));
}
