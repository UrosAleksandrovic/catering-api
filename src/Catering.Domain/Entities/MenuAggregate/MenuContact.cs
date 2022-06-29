using Ardalis.GuardClauses;

namespace Catering.Domain.Entities.MenuAggregate;

public class MenuContact : IContact
{
    public string PhoneNumber { get; private set; }

    public string Email { get; private set; }

    public string Address { get; private set; }

    public string IdentityId { get; private set; }

    private MenuContact() { }

    public MenuContact(string phoneNumber,
                       string email,
                       string address,
                       string identityId)
    {
        ValidateGeneralData(phoneNumber, email, identityId);

        PhoneNumber = phoneNumber;
        Email = email;
        Address = address;
        IdentityId = identityId;
    }

    public void Edit(string phoneNumber, string email, string address)
    {
        ValidateGeneralData(phoneNumber, email, IdentityId);

        PhoneNumber = phoneNumber;
        Email = email;
        Address = address;
    }

    private void ValidateGeneralData(string phoneNumber, string email, string identityId)
    {
        if (string.IsNullOrWhiteSpace(phoneNumber) && string.IsNullOrWhiteSpace(email))
            throw new ArgumentException($"One of following must not be null: {nameof(phoneNumber)}, {nameof(email)}");

        Guard.Against.NullOrWhiteSpace(identityId);
    }
}
