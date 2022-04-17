using Catering.Application.Aggregates.Carts.Abstractions;
using Catering.Application.Aggregates.Orders.Requests;
using Catering.Domain.Entities.CartAggregate;
using MediatR;

namespace Catering.Application.Handlers;

internal class GetCartFromCustomerHandler : IRequestHandler<GetCartFromCustomer, Cart>
{
    private readonly ICartRepository _cartRepository;

    public GetCartFromCustomerHandler(ICartRepository cartRepository)
    {
        _cartRepository = cartRepository;
    }

    public Task<Cart> Handle(GetCartFromCustomer request, CancellationToken cancellationToken)
    {
        return _cartRepository.GetByCustomerIdAsync(request.CustomerId);
    }
}
