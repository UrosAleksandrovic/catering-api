using Catering.Api.Configuration.Authorization;
using Catering.Application.Aggregates.Expenses;
using Catering.Application.Aggregates.Expenses.Abstractions;
using Catering.Application.Aggregates.Expenses.Dtos;
using Microsoft.AspNetCore.Mvc;

namespace Catering.Api.Controllers;

[Route("api/expenses")]
public class ExpensesController(IExpensesManagementAppService expensesService) : ControllerBase
{
    private readonly IExpensesManagementAppService _expensesService = expensesService;

    private const string GetNameRoute = "GetExpenseById";

    [HttpPost]
    [AuthorizeClientsAdmins]
    public async Task<IActionResult> CreateAsync([FromBody] CreateExpenseDto createRequest)
    {
        var createdId = await _expensesService.CreateAsync(createRequest);

        return CreatedAtRoute(GetNameRoute, new { id = createdId }, new { id = createdId });
    }

    [HttpPut("{id:Guid}")]
    [AuthorizeClientsAdmins]
    public async Task<IActionResult> UpdateAsync([FromRoute] Guid id, [FromBody] UpdateExpenseDto updateRequest)
    {
        await _expensesService.UpdateAsync(id, updateRequest);

        return NoContent();
    }

    [HttpGet("{id:Guid}", Name = GetNameRoute)]
    [AuthorizeClientsAdmins]
    public async Task<IActionResult> GetByIdAsync([FromRoute] Guid id)
    {
        var expense = await _expensesService.GetByIdAsync(id, this.GetUserId());

        if (expense == default)
            return NotFound();

        return Ok(expense);
    }

    [HttpGet]
    [AuthorizeClientsAdmins]
    public async Task<IActionResult> GetFiltered([FromQuery] ExpensesFilter filter)
    {
        var expenses = await _expensesService.GetFilteredAsync(filter);

        return Ok(expenses);
    }
}
