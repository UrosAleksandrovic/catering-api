using Catering.Domain.Entities.IdentityAggregate;
using MediatR;

namespace Catering.Application.Aggregates.Orders.Requests;

public class GetOrderCustomer : IRequest<Customer>
{
    public string CustomerId { get; init; }
}
