namespace Catering.Domain.Entities.MenuAggregate;

public class MenuContact : IContact
{
    public string PhoneNumber { get; private set; }

    public string Email { get; private set; }

    public string Address { get; private set; }

    private MenuContact() { }

    public MenuContact(string phoneNumber, string email, string address)
    {
        ValidateGeneralData(phoneNumber, email);

        PhoneNumber = phoneNumber;
        Email = email;
        Address = address;
    }

    private void ValidateGeneralData(string phoneNumber, string email)
    {
        if (string.IsNullOrWhiteSpace(phoneNumber) && string.IsNullOrWhiteSpace(email))
            throw new ArgumentNullException($"One of following must not be null: {nameof(phoneNumber)}; {nameof(email)}");
    }

    public void Edit(string phoneNumber, string email, string address)
    {
        ValidateGeneralData(phoneNumber, email);

        PhoneNumber = phoneNumber;
        Email = email;
        Address = address;
    }
}
