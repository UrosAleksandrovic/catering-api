using Catering.Domain.Entities.MenuAggregate;
using System;
using Xunit;

namespace Catering.Domain.Test.MenuAggregate;

public class MenuTest
{
    [Fact]
    public void Edit_NameIsNull_ArgumentNullException()
    {
        //Arrange
        var menu = new Menu("Test");

        //Act
        void a() => menu.Edit(null);

        //Assert
        Assert.Throws<ArgumentNullException>(a);
    }

    [Theory]
    [InlineData(" ")]
    [InlineData("")]
    public void Edit_NameIsInvalid_ArgumentException(string invalidName)
    {
        //Arrange
        var menu = new Menu("Test");

        //Act
        void a() => menu.Edit(invalidName);

        //Assert
        Assert.Throws<ArgumentException>(a);
    }

    [Fact]
    public void AddOrEditContact_ContacAlreadyExists_ContactIsEdited()
    {
        //Arrange
        string newPhoneNumber, newEmail, newAddress;
        newPhoneNumber = newEmail = newAddress = "SomethingNew";

        var menu = new Menu("Test");
        menu.AddOrEditContact("123", "email@email.com", "Some Address", Guid.NewGuid().ToString());

        //Act
        menu.AddOrEditContact(newPhoneNumber, newEmail, newAddress);

        //Assert
        Assert.Equal(newPhoneNumber, menu.Contact.PhoneNumber);
        Assert.Equal(newEmail, menu.Contact.Email);
        Assert.Equal(newAddress, menu.Contact.Address);
    }

    [Fact]
    public void AddOrEditContact_ContactDoesNotExist_ContactIsInitialized()
    {
        //Arrange
        string newPhoneNumber, newEmail, newAddress;
        newPhoneNumber = newEmail = newAddress = "SomethingNew";
        var menu = new Menu("Test");

        //Act
        menu.AddOrEditContact(newPhoneNumber, newEmail, newAddress, Guid.NewGuid().ToString());

        //Assert
        Assert.Equal(newPhoneNumber, menu.Contact.PhoneNumber);
        Assert.Equal(newEmail, menu.Contact.Email);
        Assert.Equal(newAddress, menu.Contact.Address);
    }

    [Theory]
    [InlineData("")]
    [InlineData(" ")]
    [InlineData(null)]
    public void AddOrEditContact_ContactDoesNotExistInvalidIdentity_ArgumentException(string invalidInput)
    {
        //Arrange
        string newPhoneNumber, newEmail, newAddress;
        newPhoneNumber = newEmail = newAddress = "SomethingNew";
        var menu = new Menu("Test");

        //Act
        void a() => menu.AddOrEditContact(newPhoneNumber, newEmail, newAddress, invalidInput);

        //Assert
        Assert.ThrowsAny<ArgumentException>(a);
    }
}
