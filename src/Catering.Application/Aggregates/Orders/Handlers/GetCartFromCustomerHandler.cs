using Catering.Application.Aggregates.Carts.Abstractions;
using Catering.Application.Aggregates.Orders.Requests;
using Catering.Domain.Aggregates.Cart;
using MediatR;

namespace Catering.Application.Aggregates.Orders.Handlers;

internal class GetCartFromCustomerHandler : IRequestHandler<GetCartFromCustomer, Cart>
{
    private readonly ICartRepository _cartRepository;

    public GetCartFromCustomerHandler(ICartRepository cartRepository)
    {
        _cartRepository = cartRepository;
    }

    public Task<Cart> Handle(GetCartFromCustomer request, CancellationToken cancellationToken)
        => _cartRepository.GetByCustomerIdAsync(request.CustomerId);
}
