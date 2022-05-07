using AutoMapper;
using Catering.Application.Aggregates.Carts.Abstractions;
using Catering.Application.Aggregates.Carts.Dtos;
using Catering.Application.Aggregates.Carts.Requests;
using Catering.Domain.Entities.CartAggregate;
using Catering.Domain.Entities.ItemAggregate;
using MediatR;

namespace Catering.Application.Aggregates.Carts;

internal class CartManagementAppService : ICartManagementAppService
{
    private readonly ICartRepository _cartRepository;
    private readonly IMediator _publisher;
    private readonly IMapper _mapper;

    public CartManagementAppService(
        ICartRepository cartRepository,
        IMapper mapper,
        IMediator publisher)
    {
        _cartRepository = cartRepository;
        _mapper = mapper;
        _publisher = publisher;
    }

    public async Task AddItemAsync(string customerId, Guid itemId, int quantity = 1, string note = null)
    {
        var cart = await GetOrCreteCartForCustomerAsync(customerId);

        cart.AddItem(itemId, quantity, note);

        await _cartRepository.UpdateAsync(cart);
    }

    public async Task AddOrEditItemNoteAsync(string customerId, Guid itemId, string note)
    {
        var cart = await GetOrCreteCartForCustomerAsync(customerId);

        cart.AddNoteToItem(itemId, note);

        await _cartRepository.UpdateAsync(cart);
    }

    public async Task DecrementItemAsync(string customerId, Guid itemId, int quantity = 1)
    {
        var cart = await GetOrCreteCartForCustomerAsync(customerId);

        cart.DecrementOrDeleteItem(itemId, quantity);

        await _cartRepository.UpdateAsync(cart);
    }

    public async Task<CartInfoDto> GetCartByCustomerIdAsync(string customerId)
    {
        var cart = await GetOrCreteCartForCustomerAsync(customerId);
        var cartItems = await _publisher.Send(new GetItemsFromTheCart { CustomerId = customerId });

        var resultCart = _mapper.Map<CartInfoDto>(cart);
        resultCart.Items = cart.Items.Select(i => MapToCartItemInfo(i, cartItems.Single(ci => ci.Id == i.ItemId)));

        return resultCart;
    }

    public async Task IncrementItemAsync(string customerId, Guid itemId, int quantity = 1)
    {
        var cart = await GetOrCreteCartForCustomerAsync(customerId);

        cart.IncrementItem(itemId, quantity);

        await _cartRepository.UpdateAsync(cart);
    }

    private async Task<Cart> GetOrCreteCartForCustomerAsync(string customerId)
    {
        var cart = await _cartRepository.GetByCustomerIdAsync(customerId);
        if (cart != default)
            return cart;

        cart = new Cart(customerId);
        await _cartRepository.CreateAsync(cart);

        return cart;
    }

    private CartItemInfoDto MapToCartItemInfo(CartItem cartItem, Item item)
    {
        var result = _mapper.Map<CartItemInfoDto>(cartItem);

        result.Price = item.Price;
        result.Name = item.Name;
        result.Description = item.Description;

        return result;
    }
}
