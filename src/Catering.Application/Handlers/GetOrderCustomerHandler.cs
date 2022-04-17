using Catering.Application.Aggregates.Identites.Abstractions;
using Catering.Application.Aggregates.Orders.Requests;
using Catering.Domain.Entities.IdentityAggregate;
using MediatR;

namespace Catering.Application.Handlers;

internal class GetOrderCustomerHandler : IRequestHandler<GetOrderCustomer, Customer>
{
    private readonly ICustomerRepository _customerRepository;

    public GetOrderCustomerHandler(ICustomerRepository customerRepository)
    {
        _customerRepository = customerRepository;
    }

    public Task<Customer> Handle(GetOrderCustomer request, CancellationToken cancellationToken)
    {
        return _customerRepository.GetByIdAsync(request.CustomerId);
    }
}
