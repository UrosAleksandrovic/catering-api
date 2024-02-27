using Catering.Application.Aggregates.Carts.Dtos;
using Catering.Application.Results;

namespace Catering.Application.Aggregates.Carts.Abstractions;

public interface ICartManagementAppService
{
    Task<Result> AddItemAsync(string customerId, AddItemToCartDto addItemDto);
    Task<Result> ChangeQuantity(string customerId, Guid itemId, int quantity);
    Task<Result> AddOrEditItemNoteAsync(string customerId, Guid itemId, string note);
    Task<Result<CartInfoDto>> GetCartByCustomerIdAsync(string customerId);
}
