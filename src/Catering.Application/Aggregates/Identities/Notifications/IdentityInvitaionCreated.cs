using MediatR;

namespace Catering.Application.Aggregates.Identities.Notifications;

internal class IdentityInvitationCreated : INotification
{
    public string InvitationId { get; set; }
}
