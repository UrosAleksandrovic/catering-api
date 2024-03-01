using Catering.Api.Configuration.Authorization;
using Catering.Api.Extensions;
using Catering.Application.Aggregates.Identities.Abstractions;
using Catering.Application.Aggregates.Identities.Dtos;
using Microsoft.AspNetCore.Mvc;

namespace Catering.Api.Controllers;

[Route("/api/identities/internals")]
public class InternalIdentitiesController(ICustomerManagementAppService customerAppService) : ControllerBase
{
    private readonly ICustomerManagementAppService _customerAppService = customerAppService;

    [HttpPost]
    [AuthorizeClientsAdmins]
    public async Task<IActionResult> RegisterAsync([FromBody] CreateCustomerDto createRequest)
        => this.FromResult(await _customerAppService.CreateClientsCustomerAsync(createRequest, this.GetUserId()));
}
