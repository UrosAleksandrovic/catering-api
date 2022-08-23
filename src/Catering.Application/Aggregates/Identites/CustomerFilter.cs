using Catering.Domain.Entities.IdentityAggregate;

namespace Catering.Application.Aggregates.Identites;

public class CustomersFilter : FilterBase
{
    public string Email { get; set; }
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public bool? IsAdministrator { get; set; }
    public IdentityRole? Role { get; set; }
    public decimal? MaxBalance { get; set; }
    public decimal? MinBalance { get; set; }
}
