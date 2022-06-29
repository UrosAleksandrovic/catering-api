using Catering.Application.Aggregates.Identites.Abstractions;
using Catering.Application.Aggregates.Identites.Dtos;
using Catering.Application.Security;
using Catering.Domain.Entities.IdentityAggregate;

namespace Catering.Application.Aggregates.Identites;

internal class ExternalIdentitiesManagementAppService : IExternalIdentitiesManagementAppService
{
    private readonly IExternalIdentityRepository _externalIdentityRepository;
    private readonly IDataProtector _dataProtector;

    public ExternalIdentitiesManagementAppService(
        IExternalIdentityRepository externalIdentityRepository,
        IDataProtector dataProtector)
    {
        _externalIdentityRepository = externalIdentityRepository;
        _dataProtector = dataProtector;
    }

    public async Task<string> CreateRestourantIdentityAsync(CreateRestourantDto createRequest)
    {
        var restourantExists = await _externalIdentityRepository.GetByEmailAsync(createRequest.Email);
        if (restourantExists != null)
            throw new ArgumentException("Restourant with provided email already exists.");

        var restourantToCreate = new ExternalIdentity(
            createRequest.Email,
            new FullName(createRequest.Name),
            _dataProtector.Hash(createRequest.InitialPassword),
            IdentityRole.RestourantEmployee);

        await _externalIdentityRepository.CreateAsync(restourantToCreate);

        return restourantToCreate.Id;
    }
}
