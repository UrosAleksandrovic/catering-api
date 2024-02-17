namespace Catering.Domain.Aggregates.Menu;

public interface IContact
{
    string PhoneNumber { get; }
    string Email { get; }
    string Address { get; }

    void Edit(string phoneNumber, string email, string address);
}
