using Catering.Application.Aggregates.Carts.Dtos;

namespace Catering.Application.Aggregates.Carts.Abstractions;

public interface ICartManagementAppService
{
    Task<CartInfoDto> GetCartByCustomerIdAsync(string customerId);
    Task AddItemAsync(string customerId, AddItemToCartDto addItemDto);
    Task ChangeQuantity(string customerId, Guid itemId, int quantity);
    Task AddOrEditItemNoteAsync(string customerId, Guid itemId, string note);
}
