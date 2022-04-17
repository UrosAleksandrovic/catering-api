using Ardalis.GuardClauses;

namespace Catering.Domain.Entities.IdentityAggregate;

public class ExternalIdentity : Identity
{
    private string _password;

    public ExternalIdentity(string email, string fullName, string password, IdentityPermissions permissions)
        : base(fullName, email, permissions)
    {
        Guard.Against.NullOrWhiteSpace(password);

        _password = password;
    }

    public bool ComparePassword(string password)
        => _password.Equals(password);

    public void EditPassword(string newPassword)
    {
        Guard.Against.NullOrWhiteSpace(newPassword);

        _password = newPassword;
    }
}
