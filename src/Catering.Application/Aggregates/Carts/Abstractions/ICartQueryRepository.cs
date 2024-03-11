using Catering.Application.Aggregates.Carts.Dtos;

namespace Catering.Application.Aggregates.Carts.Abstractions;

public interface ICartQueryRepository
{
    Task<bool> ExistsAsync(string customerId);
    Task<CartInfoDto> GetByCustomerIdAsync(string customerId);
}
