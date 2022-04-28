using MediatR;

namespace Catering.Application.Aggregates.Menus.Notifications;

public class MenuDeleted : INotification
{
    public Guid MenuId { get; init; }
}