using Ardalis.GuardClauses;
using Catering.Domain.Exceptions;

namespace Catering.Domain.Entities.IdentityAggregate;

public class Identity : BaseEntity<string>
{
    public string Email { get; private set; }
    public string FullName { get; private set; }
    public IdentityPermissions Permissions { get; private set; }

    private Identity() { }

    public Identity(string fullName, string email, IdentityPermissions permissions)
    {
        Guard.Against.NullOrWhiteSpace(email);

        Id = Guid.NewGuid().ToString();
        Email = email;
        FullName = fullName;
        Permissions = permissions;
    }

    public void Edit(string email, string fullName)
    {
        Guard.Against.NullOrWhiteSpace(email);

        Email = email;
        FullName = fullName;
    }

    public void EditOtherIdentity(
        Identity identity,
        string email,
        string fullName,
        IdentityPermissions permissions)
    {
        Guard.Against.Default(identity);

        if (Permissions != IdentityPermissions.CompanyAdministrator)
            throw new IdentityRestrictionException(Id, nameof(EditOtherIdentity));

        identity.Permissions = permissions;
        identity.Edit(email, fullName);
    }
}
