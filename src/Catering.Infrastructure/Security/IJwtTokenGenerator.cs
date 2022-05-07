using Catering.Domain.Entities.IdentityAggregate;

namespace Catering.Infrastructure.Security;

internal interface IJwtTokenGenerator
{
    string GenerateToken<T>(T identity) where T : Identity;
}
