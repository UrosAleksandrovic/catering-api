namespace Catering.Application.Aggregates.Identites.Dtos;

public class CustomerInfoDto : IdentityInfoDto
{
    public CustomerBudgetInfoDto CustomerBudgetInfo { get; set; }
}
