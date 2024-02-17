using Ardalis.GuardClauses;

namespace Catering.Domain.Aggregates.Menu;

public class Menu : ISoftDeletable
{
    public Guid Id { get; private set; }
    public string Name { get; private set; }
    public MenuContact Contact { get; private set; }

    public bool IsDeleted { get; private set; }

    private Menu() { }

    public Menu(string name)
    {
        ValidateGeneralData(name);

        Id = Guid.NewGuid();
        Name = name;
    }

    public void Edit(string name)
    {
        ValidateGeneralData(name);

        Name = name;
    }

    public void AddOrEditContact(
        string phoneNumber,
        string email,
        string address,
        string identityId = null)
    {
        if (Contact != default)
        {
            Contact.Edit(phoneNumber, email, address);
            return;
        }

        Contact = new MenuContact(phoneNumber, email, address, identityId);
    }

    public bool HasContact(string identityId)
    {
        return Contact?.IdentityId == identityId;
    }

    private void ValidateGeneralData(string name)
    {
        Guard.Against.NullOrWhiteSpace(name);
    }

    public void MarkAsDeleted() => IsDeleted = true;
}
