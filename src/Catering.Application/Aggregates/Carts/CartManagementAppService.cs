using AutoMapper;
using Catering.Application.Aggregates.Carts.Abstractions;
using Catering.Application.Aggregates.Carts.Dtos;
using Catering.Application.Aggregates.Carts.Requests;
using Catering.Application.Results;
using Catering.Application.Validation;
using Catering.Domain.Aggregates.Cart;
using Catering.Domain.Aggregates.Item;
using Catering.Domain.Exceptions;
using MediatR;

namespace Catering.Application.Aggregates.Carts;

internal class CartManagementAppService : ICartManagementAppService
{
    private readonly ICartRepository _cartRepository;
    private readonly IValidationProvider _validationProvider;
    private readonly IMapper _mapper;
    private readonly IMediator _publisher;

    public CartManagementAppService(
        ICartRepository cartRepository,
        IValidationProvider validationProvider,
        IMapper mapper,
        IMediator publisher)
    {
        _cartRepository = cartRepository;
        _mapper = mapper;
        _publisher = publisher;
        _validationProvider = validationProvider;
    }

    public async Task<Result> AddItemAsync(string customerId, AddItemToCartDto addItemDto)
    {
        if (await _validationProvider.ValidateModelAsync(addItemDto) is var valResult && !valResult.IsSuccess)
            return valResult;

        var cart = await GetOrCreteCartForCustomerAsync(customerId);

        cart.AddItem(addItemDto.MenuId, addItemDto.ItemId, addItemDto.Quantity, addItemDto.Note);

        await _cartRepository.UpdateAsync(cart);
        return Result.Success();
    }

    public async Task<Result> AddOrEditItemNoteAsync(string customerId, Guid itemId, string note)
    {
        var cart = await GetOrCreteCartForCustomerAsync(customerId);

        cart.AddOrEditNoteToItem(itemId, note);

        await _cartRepository.UpdateAsync(cart);
        return Result.Success();
    }

    public async Task<Result> ChangeQuantity(string customerId, Guid itemId, int quantity)
    {
        var cart = await GetOrCreteCartForCustomerAsync(customerId);

        var currentQuantity = cart.GetItemQuantity(itemId);
        if (currentQuantity == 0)
            throw new ItemNotInCartException(itemId, cart.Id);

        if (quantity > currentQuantity)
            cart.IncrementItem(itemId, quantity - currentQuantity);
        else
            cart.DecrementOrDeleteItem(itemId, currentQuantity - quantity);

        await _cartRepository.UpdateAsync(cart);
        return Result.Success();
    }

    public async Task<Result<CartInfoDto>> GetCartByCustomerIdAsync(string customerId)
    {
        var cart = await GetOrCreteCartForCustomerAsync(customerId);
        var cartItems = await _publisher.Send(new GetItemsFromTheCart(customerId));

        var resultCart = _mapper.Map<CartInfoDto>(cart);
        resultCart.Items = cart.Items.Select(i => MapToCartItemInfo(i, cartItems.SingleOrDefault(ci => ci.Id == i.ItemId)));

        return Result.Success();
    }

    private async Task<Cart> CreateForCustomerAsync(string customerId)
    {
        var cart = new Cart(customerId);
        await _cartRepository.CreateAsync(cart);

        return cart;
    }

    private async Task<Cart> GetOrCreteCartForCustomerAsync(string customerId)
    {
        var cart = await _cartRepository.GetByCustomerIdAsync(customerId);
        if (cart != default)
            return cart;

        cart = await CreateForCustomerAsync(customerId);

        return cart;
    }

    private CartItemInfoDto MapToCartItemInfo(CartItem cartItem, Item item)
    {
        if (item == null)
            return null;

        var result = _mapper.Map<CartItemInfoDto>(cartItem);

        result.Price = item.Price;
        result.Name = item.Name;
        result.Description = item.Description;

        return result;
    }
}
