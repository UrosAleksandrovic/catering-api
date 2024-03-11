using Catering.Application.Filtering;
using Catering.Domain.Aggregates.Identity;

namespace Catering.Application.Aggregates.Identities;

public class IdentityFilter : FilterBase
{
    public string Email { get; set; }
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public IdentityRole? Role { get; set; }
    public bool? IsExternal { get; set; }
    public IdentitiesOrderBy? OrderBy { get; set; }
    public bool IsOrderByDescending { get; set; }
}
