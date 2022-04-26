using Catering.Application.Aggregates.Identites.Abstractions;
using Catering.Application.Security;
using Catering.Domain.Entities.IdentityAggregate;
using LdapForNet;
using LdapForNet.Native;
using Microsoft.Extensions.Options;
using System.Security.Authentication;

namespace Catering.Infrastructure.Security.Ldap;

internal class LdapTokenAuthenticator : ITokenAtuhenticator<Customer>
{
    private readonly SecurityLdapSettings _ldapOptions;
    private readonly ICustomerRepository _customerRepository;
    private readonly IJwtTokenGenerator _tokenGenerator;

    public LdapTokenAuthenticator(
        IOptions<SecurityLdapSettings> ldapOptions,
        ICustomerRepository customerRepository,
        IJwtTokenGenerator tokenGenerator)
    {
        _ldapOptions = ldapOptions.Value;
        _customerRepository = customerRepository;
        _tokenGenerator = tokenGenerator;
    }

    public async Task<Customer> AuthenticateAsync(string identity, string password)
    {
        var ldapUser = RetrieveLdapUser(identity, password);

        var registeredCustomer = await _customerRepository.GetByEmailAsync(ldapUser.Mail);
        if (registeredCustomer == default)
            throw new AuthenticationException();

        await PopulateCustomerAsync(registeredCustomer, ldapUser);

        return registeredCustomer;
    }

    public async Task<string> GenerateTokenAsync(string identity, string password)
    {
        var externalIdentity = await AuthenticateAsync(identity, password);

        return _tokenGenerator.GenerateToken(externalIdentity);
    }

    private LdapUser RetrieveLdapUser(string username, string password)
    {
        using var connection = new LdapConnection();

        connection.Connect(_ldapOptions.Server);

        connection.Bind(Native.LdapAuthMechanism.SIMPLE, GetUserBaseDn(username), password);

        var searchResult = connection.Search(GetUserBaseDn(username), "(objectClass=*)").FirstOrDefault();

        return new LdapUser(searchResult);
    }

    private string GetUserBaseDn(string username, bool useCn = false)
    {
        var qualifier = useCn ? "CN" : "uid";
        return $"{qualifier}={username},{_ldapOptions.DirectoryPath}";
    }

    private async Task PopulateCustomerAsync(Customer customer, LdapUser ldapUser)
    {
        if (customer.FullName.FirstName != default)
            return;
            
        customer.FullName.Edit(ldapUser.FirstName, ldapUser.LastName);

        await _customerRepository.UpdateAsync(customer);
    }
}
