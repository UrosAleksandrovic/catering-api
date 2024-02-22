using Ardalis.GuardClauses;

namespace Catering.Domain.Aggregates.Identity;

public class FullName
{
    public string FirstName { get; private set; }
    public string LastName { get; private set; }

    private FullName() { }

    public FullName(string firstName, string lastName = null)
    {
        FirstName = firstName.Trim();
        LastName = lastName?.Trim();
    }

    public FullName(FullName fullName)
    {
        FirstName = fullName.FirstName;
        LastName = fullName.LastName;
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
