using Catering.Application.Aggregates.Identities.Abstractions;
using Catering.Application.Aggregates.Identities.Dtos;
using Catering.Application.Security;
using Catering.Domain.Entities.IdentityAggregate;

namespace Catering.Application.Aggregates.Identities;

internal class CateringIdentitiesManagementAppService : ICateringIdentitiesManagementAppService
{
    private readonly ICateringIdentitiesRepository _cateringIdentitiesRepository;
    private readonly IDataProtector _dataProtector;

    public CateringIdentitiesManagementAppService(
        ICateringIdentitiesRepository cateringIdentitiesRepository,
        IDataProtector dataProtector)
    {
        _cateringIdentitiesRepository = cateringIdentitiesRepository;
        _dataProtector = dataProtector;
    }

    public async Task<string> CreateRestaurantAsync(CreateRestaurantDto createRequest)
    {
        var restaurantExists = await _cateringIdentitiesRepository.GetByEmailAsync(createRequest.Email);
        if (restaurantExists != null)
            throw new ArgumentException("Restaurant with provided email already exists.");

        var restaurantToCreate = new CateringIdentity(
            createRequest.Email,
            new FullName(createRequest.Name),
            _dataProtector.Hash(createRequest.InitialPassword),
            IdentityRole.Employee | IdentityRole.Restaurant);

        await _cateringIdentitiesRepository.CreateAsync(restaurantToCreate);

        return restaurantToCreate.Id;
    }
}
