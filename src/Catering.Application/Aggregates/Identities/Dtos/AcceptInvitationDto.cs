namespace Catering.Application.Aggregates.Identities.Dtos;

public class AcceptInvitationDto
{
    public bool HasAccepted { get; set; }
    public string NewPassword { get; set; }
}
