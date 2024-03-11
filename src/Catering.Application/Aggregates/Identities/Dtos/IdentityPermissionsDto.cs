using Catering.Domain.Aggregates.Identity;

namespace Catering.Application.Aggregates.Identities.Dtos;

public class IdentityPermissionsDto
{
    public string Id { get; set; }
    public string Email { get; set; }
    public IdentityRole Role { get; set; }
    public FullName FullName { get; set; }
    public string[] Permissions { get; set; }
}
