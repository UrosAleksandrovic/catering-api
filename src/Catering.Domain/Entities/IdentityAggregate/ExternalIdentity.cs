using Ardalis.GuardClauses;

namespace Catering.Domain.Entities.IdentityAggregate;

public class ExternalIdentity : Identity
{
    private string _password;

    protected ExternalIdentity() : base() { }

    public ExternalIdentity(
        string email,
        FullName fullName,
        string password,
        string startRole)
        : base(fullName, email, startRole)
    {
        Guard.Against.NullOrWhiteSpace(password);

        _password = password;
    }

    public bool ComparePassword(string password)
        => _password.Equals(password);

    public void EditPassword(string newPassword)
    {
        Guard.Against.NullOrWhiteSpace(newPassword);

        _password = newPassword.Trim();
    }
}
