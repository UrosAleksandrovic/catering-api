using Catering.Api.Configuration.Authorization;
using Catering.Api.Extensions;
using Catering.Application.Aggregates.Expenses;
using Catering.Application.Aggregates.Expenses.Abstractions;
using Catering.Application.Aggregates.Expenses.Dtos;
using Catering.Application.Aggregates.Expenses.Queries;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Catering.Api.Controllers;

[Route("api/expenses")]
public class ExpensesController(IExpensesManagementAppService expensesService, IMediator publisher) : ControllerBase
{
    private readonly IExpensesManagementAppService _expensesService = expensesService;
    private readonly IMediator _publisher = publisher;

    private const string GetNameRoute = "GetExpenseById";

    [HttpPost]
    [AuthorizeClientsAdmins]
    public async Task<IActionResult> CreateAsync([FromBody] CreateExpenseDto createRequest)
        => this.CreatedAtRouteFromResult(await _expensesService.CreateAsync(createRequest), GetNameRoute);

    [HttpPut("{id:Guid}")]
    [AuthorizeClientsAdmins]
    public async Task<IActionResult> UpdateAsync([FromRoute] Guid id, [FromBody] UpdateExpenseDto updateRequest)
        => this.FromResult(await _expensesService.UpdateAsync(id, updateRequest));

    [HttpGet("{id:Guid}", Name = GetNameRoute)]
    [AuthorizeClientsAdmins]
    public async Task<IActionResult> GetByIdAsync([FromRoute] Guid id)
        => this.FromResult(await _publisher.Send(new GetExpenseByIdQuery(id, this.GetUserId())));

    [HttpGet]
    [AuthorizeClientsAdmins]
    public async Task<IActionResult> GetFiltered([FromQuery] ExpensesFilter filters)
        => this.FromResult(await _publisher.Send(new GetFilteredExpensesQuery(filters)));
}
