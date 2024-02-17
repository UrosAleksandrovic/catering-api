using Ardalis.GuardClauses;
using Catering.Domain.ErrorCodes;
using Catering.Domain.Exceptions;

namespace Catering.Domain.Aggregates.Identity;

public class IdentityInvitation
{
    public string Id { get; private set; }
    public string Email { get; private set; }
    public FullName FullName { get; private set; }
    public DateTime CreatedOn { get; private set; }
    public DateTime ExpiredOn { get; private set; }
    public IdentityRole FutureRole { get; private set; }
    public bool IsCustomer { get; private set; }

    protected IdentityInvitation() { }

    public IdentityInvitation(
        string email,
        FullName fullName,
        int daysToExpire,
        IdentityRole futureRole,
        bool isCustomer)
    {
        Guard.Against.NullOrWhiteSpace(email);
        Guard.Against.Zero((int)futureRole);

        Id = Guid.NewGuid().ToString();

        Email = email;
        FullName = fullName;
        FutureRole = futureRole;

        CreatedOn = DateTime.Now;
        ExpiredOn = CreatedOn.AddDays(daysToExpire);
        IsCustomer = isCustomer;
    }

    public (Identity identity, Customer customer) AcceptInvitation(string password)
    {
        if (DateTime.Now.CompareTo(ExpiredOn) > 0)
            throw new CateringException(IdentityErrorCodes.INVITATION_EXPIRED);

        var identity = new CateringIdentity(Email, FullName, password, FutureRole);

        return (identity, IsCustomer ? new Customer(identity) : null);
    }
}
