﻿using Ardalis.GuardClauses;

namespace Catering.Domain.Entities.MenuAggregate;

public class Menu : BaseEntity
{
    public string Name { get; private set; }
    public IContact Contact { get; private set; }

    public Menu(string name)
    {
        ValidateGeneralData(name);

        Id = Guid.NewGuid();
        Name = name;
    }

    public void AddOrEditContact(string phoneNumber, string email, string address)
    {
        if (Contact != default)
        {
            Contact.Edit(phoneNumber, email, address);
            return;
        }

        Contact = new MenuContact(phoneNumber, email, address);
    }

    private void ValidateGeneralData(string name)
    {
        Guard.Against.NullOrWhiteSpace(name);
    }
}