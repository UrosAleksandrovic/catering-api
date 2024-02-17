using Catering.Domain.Aggregates.Identity;

namespace Catering.Application.Aggregates.Identities.Dtos;

public class IdentityPermissionsDto
{
    public IdentityPermissions Permissions { get; set; }
    public IdentityRole Role { get; set; }
}
