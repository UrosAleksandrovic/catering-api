using Catering.Domain.Aggregates.Identity;

namespace Catering.Application.Aggregates.Identities.Dtos;

public class CreateIdentityInvitationDto
{
    public string Email { get; set; }
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public IdentityRole FutureRole { get; set; }
    public bool IsCustomer { get; set; }
}
