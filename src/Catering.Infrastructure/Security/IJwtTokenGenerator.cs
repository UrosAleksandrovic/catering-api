using Catering.Domain.Aggregates.Identity;

namespace Catering.Infrastructure.Security;

internal interface IJwtTokenGenerator
{
    string GenerateToken<T>(T identity) where T : Identity;
}
