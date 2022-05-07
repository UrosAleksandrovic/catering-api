using Catering.Application.Aggregates.Identites.Dtos;

namespace Catering.Application.Aggregates.Identites.Abstractions;

public interface IExternalIdentitiesManagementAppService
{
    public Task<string> CreateRestourantIdentityAsync(CreateRestourantDto createRestourant);
}
