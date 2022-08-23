using Catering.Application.Aggregates.Identites.Abstractions;
using Catering.Application.Security;
using Catering.Domain.Entities.IdentityAggregate;
using Catering.Infrastructure.Security.Settings;
using LdapForNet;
using LdapForNet.Native;
using Microsoft.Extensions.Options;
using System.Security.Authentication;

namespace Catering.Infrastructure.Security.Ldap;

internal class LdapTokenAuthenticator : ITokenAtuhenticator<Identity>
{
    private readonly SecurityLdapSettings _ldapOptions;
    private readonly IIdentityRepository<Identity> _identityRepository;
    private readonly IJwtTokenGenerator _tokenGenerator;

    public LdapTokenAuthenticator(
        IOptions<SecurityLdapSettings> ldapOptions,
        IIdentityRepository<Identity> identityRepository,
        IJwtTokenGenerator tokenGenerator)
    {
        _ldapOptions = ldapOptions.Value;
        _identityRepository = identityRepository;
        _tokenGenerator = tokenGenerator;
    }

    public async Task<Identity> AuthenticateAsync(string identity, string password)
    {
        var ldapUser = RetrieveLdapUser(identity, password);

        var registeredIdentity = await _identityRepository.GetByEmailAsync(ldapUser.Mail);
        if (registeredIdentity == default)
            throw new AuthenticationException();

        await PopulateIdentityAsync(registeredIdentity, ldapUser);

        return registeredIdentity;
    }

    public async Task<string> GenerateTokenAsync(string identity, string password)
    {
        var externalIdentity = await AuthenticateAsync(identity, password);

        return _tokenGenerator.GenerateToken(externalIdentity);
    }

    private LdapUser RetrieveLdapUser(string username, string password)
    {
        using var connection = new LdapConnection();

        connection.Connect(_ldapOptions.Server, _ldapOptions.PortNumber);

        var userDn = GetUserBaseDn(username);
        connection.Bind(Native.LdapAuthMechanism.SIMPLE, userDn, password);

        var searchResult = connection.Search(GetUserBaseDn(username), "(objectClass=*)").FirstOrDefault();

        return new LdapUser(searchResult);
    }

    private string GetUserBaseDn(string username, bool useCn = false)
    {
        var qualifier = useCn ? "CN" : "uid";
        return $"{qualifier}={username},{_ldapOptions.DirectoryPath}";
    }

    private async Task PopulateIdentityAsync(Identity identity, LdapUser ldapUser)
    {
        if (identity.FullName?.FirstName != default)
            return;
            
        identity.Edit(identity.Email, new FullName(ldapUser.FirstName, ldapUser.LastName));

        await _identityRepository.UpdateAsync(identity);
    }
}
