namespace Catering.Application.Aggregates.Identites.Dtos;

public class CreateCustomerDto
{
    public string Email { get; set; }
    public bool IsAdministrator { get; set; }
    public bool IsCompanyEmployee { get; set; }
    public bool IsRestourantEmployee { get; set; }
}
