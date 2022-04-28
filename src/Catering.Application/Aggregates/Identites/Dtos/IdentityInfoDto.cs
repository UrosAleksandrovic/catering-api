namespace Catering.Application.Aggregates.Identites.Dtos;

public class IdentityInfoDto
{
    public string Id { get; set; }
    public string Email { get; set; }
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public bool IsAdministrator { get; set; }
    public bool IsCustomer { get; set; }
    public bool IsRestourantEmployee { get; set; }
}
