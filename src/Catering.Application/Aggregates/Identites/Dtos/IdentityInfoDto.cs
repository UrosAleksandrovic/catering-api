namespace Catering.Application.Aggregates.Identites.Dtos;

public class IdentityInfoDto
{
    public string Id { get; set; }
    public string Email { get; set; }
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public IEnumerable<string> Roles { get; set; }
}
