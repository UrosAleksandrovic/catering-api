using Ardalis.GuardClauses;

namespace Catering.Domain.Entities.IdentityAggregate;

public class FullName
{
    public string FirstName { get; private set; }
    public string LastName { get; private set; }

    public FullName(string firstName, string lastName = null)
    {
        FirstName = firstName.Trim();
        LastName = lastName?.Trim();
    }

    public void Edit(string firstName, string lastName = null)
    {
        Guard.Against.NullOrWhiteSpace(firstName);

        FirstName = firstName.Trim();
        LastName = lastName?.Trim();
    }

    public override string ToString()
    {
        return $"{FirstName} {LastName}".TrimEnd();
    }
}
