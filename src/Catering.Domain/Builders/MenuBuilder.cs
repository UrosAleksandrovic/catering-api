using Catering.Domain.Aggregates.Menu;

namespace Catering.Domain.Builders;

public class MenuBuilder : IBuilder<Menu>
{
    private string _name;
    private string _address;
    private string _phoneNumber;
    private string _email;
    private string _identityId;

    public Menu Build()
    {
        var result = new Menu(_name);

        if (_phoneNumber != null || _address != null || _email != null || _identityId != null)
            result.AddOrEditContact(_phoneNumber, _email, _address, _identityId);

        return result;
    }

    public MenuBuilder HasName(string name)
    {
        _name = name;

        return this;
    }

    public MenuBuilder HasContact(string phoneNumber, string email, string address)
    {
        _phoneNumber = phoneNumber;
        _address = address;
        _email = email;

        return this;
    }

    public MenuBuilder HasContactIdentity(string identityId)
    {
        _identityId = identityId;

        return this;
    }

    public void Reset()
    {
        _identityId = _name = _address = _phoneNumber = _email = default;
    }
}
