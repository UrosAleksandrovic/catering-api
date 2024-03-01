using Ardalis.GuardClauses;
using Catering.Domain.Exceptions;

namespace Catering.Domain.Aggregates.Identity;

public class Identity
{
    public string Id { get; private set; }
    public string Email { get; private set; }
    public FullName FullName { get; private set; }
    public IdentityRole Role { get; private set; }
    public bool IsExternal { get; private set; }

    protected Identity() { }

    public Identity(
        string email,
        FullName fullName,
        IdentityRole startingRole,
        bool isExternal)
        : this(email, startingRole, isExternal)
    {
        FullName = fullName;
    }

    public Identity(
        string email,
        IdentityRole startingRole,
        bool isExternal)
    {
        Guard.Against.NullOrWhiteSpace(email);
        Guard.Against.Zero((int)startingRole);

        Id = Guid.NewGuid().ToString();
        Email = email;
        Role = startingRole;
        IsExternal = isExternal;
    }

    public void Edit(string email, FullName fullName)
    {
        Guard.Against.NullOrWhiteSpace(email);

        Email = email;

        if (FullName == default && fullName != default)
            FullName = new FullName(fullName.FirstName, fullName.LastName);

        if (FullName != default && fullName != default)
            FullName.Edit(fullName.FirstName, fullName.LastName);

        if (fullName == default)
            FullName = default;
    }

    protected void ChangeRole(IdentityRole newRole)
        => Role = newRole;

    public void EditOtherIdentity(
        Identity identity,
        string email,
        FullName fullName,
        IdentityRole newRole)
    {
        Guard.Against.Default(identity);
        Guard.Against.Zero((int)newRole);

        if (!Role.IsAdministrator())
            throw new IdentityRestrictionException(Id, nameof(EditOtherIdentity));

        identity.Edit(email, fullName);
        identity.ChangeRole(newRole);
    }

    public bool HasRole(IdentityRole role)
        => Role == role;
}
