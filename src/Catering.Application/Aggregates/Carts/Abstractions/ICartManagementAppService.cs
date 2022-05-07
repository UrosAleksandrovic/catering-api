using Catering.Application.Aggregates.Carts.Dtos;

namespace Catering.Application.Aggregates.Carts.Abstractions;

public interface ICartManagementAppService
{
    Task<CartInfoDto> GetCartByCustomerIdAsync(string customerId);
    Task AddItemAsync(string customerId, Guid itemId, int quantity = 1, string note = null);
    Task IncrementItemAsync(string customerId, Guid itemId, int quantity = 1);
    Task DecrementItemAsync(string customerId, Guid itemId, int quantity = 1);
    Task AddOrEditItemNoteAsync(string customerId, Guid itemId, string note);
}
