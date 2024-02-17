using Catering.Domain.Aggregates.Identity;
using MediatR;

namespace Catering.Application.Aggregates.Orders.Requests;

public class GetOrderCustomer : IRequest<Customer>
{
    public string CustomerId { get; init; }
}
