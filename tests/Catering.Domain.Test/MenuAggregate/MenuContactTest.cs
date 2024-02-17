using System;
using Catering.Domain.Aggregates.Menu;
using Xunit;

namespace Catering.Domain.Test.MenuAggregate;

public class MenuContactTest
{
    [Theory]
    [InlineData("")]
    [InlineData(" ")]
    [InlineData(null)]
    public void Edit_BothPhoneNumberAndEmailAreInvalid_ArgumentNullException(string invalidInput)
    {
        //Arrange
        var contact = new MenuContact("Ok", "Ok", "", Guid.NewGuid().ToString());

        //Act
        void a() => contact.Edit(invalidInput, invalidInput, contact.Address);

        //Assert
        var exception = Assert.Throws<ArgumentException>(a);
        Assert.Equal("One of following must not be null: phoneNumber, email", exception.Message);
    }

    [Theory]
    [InlineData("")]
    [InlineData(" ")]
    [InlineData(null)]
    public void Edit_PhoneIsValidButEmailIsInvalid_ValidPath(string invalidInput)
    {
        //Arrange
        var expectedPhone = "123";
        var contact = new MenuContact("Ok", "Ok", "", Guid.NewGuid().ToString());

        //Act
        contact.Edit(expectedPhone, invalidInput, contact.Address);

        //Assert
        Assert.Equal(expectedPhone, contact.PhoneNumber);
        Assert.Equal(invalidInput, contact.Email);
    }

    [Theory]
    [InlineData("")]
    [InlineData(" ")]
    [InlineData(null)]
    public void Edit_PhoneIsInvalidButEmailIsValid_ValidPath(string invalidInput)
    {
        //Arrange
        var expectedEmail = "email";
        var contact = new MenuContact("Ok", "Ok", "", Guid.NewGuid().ToString());

        //Act
        contact.Edit(invalidInput, expectedEmail, contact.Address);

        //Assert
        Assert.Equal(expectedEmail, contact.Email);
        Assert.Equal(invalidInput, contact.PhoneNumber);
    }

    [Fact]
    public void Edit_ValidPath_EverythingChanged()
    {
        //Arrange
        var newEmail = "email";
        var newPhone = "123";
        var newAddress = "address";
        var contact = new MenuContact("Ok", "Ok", "", Guid.NewGuid().ToString());

        //Act
        contact.Edit(newPhone, newEmail, newAddress);

        //Assert
        Assert.Equal(newEmail, contact.Email);
        Assert.Equal(newPhone, contact.PhoneNumber);
        Assert.Equal(newAddress, contact.Address);
    }
}
