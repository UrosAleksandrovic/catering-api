namespace Catering.Domain.Entities.MenuAggregate;

public class MenuBuilder : IBuilder<Menu>
{
    private string _name;
    private string _address;
    private string _phoneNumber;
    private string _email;

    public Menu Build()
    {
        var result = new Menu(_name);

        if (_phoneNumber != null || _address != null || _email != null)
            result.AddOrEditContact(_phoneNumber, _email, _address);

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
}
