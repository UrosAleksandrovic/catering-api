using Catering.Application.Aggregates.Identites.Dtos;

namespace Catering.Application.Aggregates.Identites.Abstractions;

public interface ICateringIdentititesManagementAppService
{
    public Task<string> CreateRestourantAsync(CreateRestourantDto createRequest);
}
