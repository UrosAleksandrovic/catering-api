﻿using Catering.Application.Aggregates.Identities.Abstractions;
using Catering.Application.Aggregates.Orders.Requests;
using Catering.Domain.Aggregates.Identity;
using MediatR;

namespace Catering.Application.Aggregates.Orders.Handlers;

internal class GetOrderCustomerHandler : IRequestHandler<GetOrderCustomer, Customer>
{
    private readonly ICustomerRepository _customerRepository;

    public GetOrderCustomerHandler(ICustomerRepository customerRepository)
    {
        _customerRepository = customerRepository;
    }

    public Task<Customer> Handle(GetOrderCustomer request, CancellationToken cancellationToken)
        => _customerRepository.GetFullByIdAsync(request.CustomerId);
}
