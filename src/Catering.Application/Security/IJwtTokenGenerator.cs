using Catering.Domain.Entities.IdentityAggregate;

namespace Catering.Application.Security;

//TODO: Move this to infrastructure layer
public interface IJwtTokenGenerator
{
    string GenerateToken<T>(T identity) where T : Identity;
}
