using Ardalis.GuardClauses;
using Catering.Domain.Exceptions;

namespace Catering.Domain.Entities.IdentityAggregate;

public class Identity : BaseEntity<string>
{
    public string Email { get; private set; }
    public FullName FullName { get; private set; }

    private readonly List<string> _roles = new();
    public IReadOnlyList<string> Roles => _roles.AsReadOnly();

    protected Identity() { }

    public Identity(
        FullName fullName,
        string email,
        string startRole)
    {
        Guard.Against.NullOrWhiteSpace(email);
        Guard.Against.NullOrWhiteSpace(startRole);

        Id = Guid.NewGuid().ToString();
        Email = email;
        FullName = fullName;
        _roles.Add(startRole);
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

    protected void EditRoles(IEnumerable<string> newRoles)
    {
        Guard.Against.NullOrEmpty(newRoles);

        _roles.Clear();

        _roles.AddRange(newRoles);
    }

    public void EditOtherIdentity(
        Identity identity,
        string email,
        FullName fullName,
        IEnumerable<string> newRoles)
    {
        Guard.Against.Default(identity);

        if (!IsAdministrator)
            throw new IdentityRestrictionException(Id, nameof(EditOtherIdentity));

        identity.Edit(email, fullName);
        identity.EditRoles(newRoles);
    }

    public bool HasRole(string role)
        => _roles.Contains(role);

    public bool IsAdministrator => _roles.Any(IdentityRole.IsAdministratorRole);

    public bool IsCompanyEmployee
        => _roles.Intersect(new[]
        {
            IdentityRole.CompanyEmployee,
            IdentityRole.CompanyAdministrator
        }).Any();

    public bool IsRestourantEmployee
        => _roles.Intersect(new[]
        {
            IdentityRole.RestourantEmployee
        }).Any();
}
