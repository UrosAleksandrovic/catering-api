using MediatR;

namespace Catering.Application.Aggregates.Orders.Requests;

internal class GetMenuWithContactId : IRequest<Guid?>
{
    public string ContactId { get; set; }
}
