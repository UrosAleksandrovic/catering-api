namespace Catering.Application.Aggregates.Expenses.Dtos;

public class ExpenseInfoDto
{
    public Guid Id { get; set; }
    public string CustomerId { get; set; }
    public Guid MenuId { get; set; }
    public decimal Price { get; set; }
    public string Note { get; set; }
    public DateTimeOffset DeliveredOn { get; set; }
}
