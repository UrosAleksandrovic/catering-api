using MediatR;

namespace Catering.Application.Aggregates.Menus.Requests;

public class MenuDeleted : INotification 
{
    public Guid MenuId { get; init; }
}