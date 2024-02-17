using Ardalis.GuardClauses;

namespace Catering.Domain.Aggregates.Identity;

public class CateringIdentity : Identity
{
    private string _password;

    protected CateringIdentity() : base() { }

    public CateringIdentity(
        string email,
        FullName fullName,
        string password,
        IdentityRole startRole)
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
