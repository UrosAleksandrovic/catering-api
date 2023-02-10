using Catering.Application.Aggregates.Identities.Dtos;

namespace Catering.Application.Aggregates.Identities.Abstractions;

public interface ICateringIdentitiesManagementAppService
{
    public Task<string> CreateRestaurantAsync(CreateRestaurantDto createRequest);
}
