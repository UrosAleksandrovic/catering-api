using MediatR;

namespace Catering.Application.Aggregates.Menus.Notifications;

public record MenuDeleted(Guid MenuId) : INotification;
