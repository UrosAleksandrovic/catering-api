using Catering.Application.Aggregates.Identites.Abstractions;
using Catering.Application.Aggregates.Identites.Dtos;
using Catering.Application.Security;
using Catering.Domain.Entities.IdentityAggregate;

namespace Catering.Application.Aggregates.Identites;

internal class CateringIdentitiesManagementAppService : ICateringIdentititesManagementAppService
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

    public async Task<string> CreateRestourantAsync(CreateRestourantDto createRequest)
    {
        var restourantExists = await _cateringIdentitiesRepository.GetByEmailAsync(createRequest.Email);
        if (restourantExists != null)
            throw new ArgumentException("Restourant with provided email already exists.");

        var restourantToCreate = new CateringIdentity(
            createRequest.Email,
            new FullName(createRequest.Name),
            _dataProtector.Hash(createRequest.InitialPassword),
            IdentityRole.Employee | IdentityRole.Restourant);

        await _cateringIdentitiesRepository.CreateAsync(restourantToCreate);

        return restourantToCreate.Id;
    }
}
