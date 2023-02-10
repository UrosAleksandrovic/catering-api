namespace Catering.Application.Aggregates.Identities.Dtos;

public class CustomerBudgetInfoDto
{
    public decimal ReservedBalance { get; set; }
    public decimal Budget { get; set; }
    public decimal BudgetLeft => Budget - ReservedBalance;
}
