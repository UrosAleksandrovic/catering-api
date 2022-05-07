using Ardalis.GuardClauses;
using Catering.Application.Aggregates.Identites.Abstractions;
using Catering.Application.Security;
using Catering.Domain.Entities.IdentityAggregate;
using System.Security.Authentication;

namespace Catering.Infrastructure.Security;

internal class CateringTokenAuthenticator : ITokenAtuhenticator<ExternalIdentity>
{
    private readonly IExternalIdentityRepository _repository;
    private readonly IDataProtector _dataProtector;
    private readonly IJwtTokenGenerator _jwtTokenGenerator;

    public CateringTokenAuthenticator(
        IExternalIdentityRepository repository,
        IDataProtector dataProtector,
        IJwtTokenGenerator jwtTokenGenerator)
    {
        _repository = repository;
        _dataProtector = dataProtector;
        _jwtTokenGenerator = jwtTokenGenerator;
    }

    public async Task<ExternalIdentity> AuthenticateAsync(string identity, string password)
    {
        Guard.Against.NullOrWhiteSpace(identity);
        Guard.Against.NullOrWhiteSpace(password);

        var dbIdentity= await _repository.GetByEmailAsync(identity);
        if (dbIdentity == default)
            throw new AuthenticationException();

        var isAuthenticated = dbIdentity.ComparePassword(_dataProtector.Hash(password));

        if (!isAuthenticated)
            throw new AuthenticationException();

        return dbIdentity;
    }

    public async Task<string> GenerateTokenAsync(string identity, string password)
    {
        var externalIdentity = await AuthenticateAsync(identity, password);

        return _jwtTokenGenerator.GenerateToken(externalIdentity);
    }
}
