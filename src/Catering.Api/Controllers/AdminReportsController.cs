using Catering.Api.Configuration.Authorization;
using Catering.Api.Extensions;
using Catering.Application.Aggregates.Identities.Queries;
using Catering.Domain;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Catering.Api.Controllers;

[Route("/api/admin/reports")]
[AuthorizeClientsAdmins]
public class AdminReportsController(IMediator mediator) : ControllerBase
{
    private readonly IMediator _mediator = mediator;

    [HttpGet("monthly-spendings")]
    public async Task<IActionResult> GetMonthlySendings([FromQuery] YearAndMonth yearAndMonth)
        => this.FromResult(await _mediator.Send(new GetCustomerMonthlySendingsQuery(yearAndMonth)));
}
