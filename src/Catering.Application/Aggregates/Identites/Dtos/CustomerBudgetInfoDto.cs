namespace Catering.Application.Aggregates.Identites.Dtos;

public class CustomerBudgetInfoDto
{
    public decimal ReservedBalance { get; set; }
    public decimal Budget { get; set; }
    public decimal BudgetLeft => Budget - ReservedBalance;
}
