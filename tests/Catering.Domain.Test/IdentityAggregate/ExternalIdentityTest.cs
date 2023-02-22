using Catering.Domain.Entities.IdentityAggregate;
using System;
using System.Linq;
using Xunit;

namespace Catering.Domain.Test.CustomerAggregate;

public class ExternalIdentityTest
{
    [Theory]
    [InlineData("")]
    [InlineData(" ")]
    [InlineData(null)]
    public void ComparePassword_PassedPasswordIsNullOrEmpty_EvaluatesFalse(string invalidPassword)
    {
        //Arrange
        var externalIdentity = new CateringIdentity(
            "Some Email",
            new FullName("Test", "Test"),
            "SomePassword@123",
            IdentityRoleExtensions.GetRestaurantEmployee());

        //Act
        var result = externalIdentity.ComparePassword(invalidPassword);

        //Assert
        Assert.False(result);
    }

    [Fact]
    public void ComparePassword_PassedPasswordIsSameLength_EvaluatesFalse()
    {
        //Arrange
        var password = "SomePassword@123";
        var externalIdentity = new CateringIdentity(
            "Some Email",
            new FullName("Test", "Test"),
            password,
            IdentityRoleExtensions.GetRestaurantEmployee());

        //Act
        var result = externalIdentity.ComparePassword(string.Join("", Enumerable.Repeat('s', password.Length)));

        //Assert
        Assert.False(result);
    }

    [Fact]
    public void ComparePassword_PasswordHasSameLetters_EvaluatesFalse()
    {
        //Arrange
        var password = "SomePassword@123";
        var externalIdentity = new CateringIdentity(
            "Some Email",
            new FullName("Test", "Test"),
            password,
            IdentityRoleExtensions.GetRestaurantEmployee());

        //Act
        password = "somePassword@123";
        var result = externalIdentity.ComparePassword(password);

        //Assert
        Assert.False(result);
    }

    [Fact]
    public void ComparePassword_PasswordIsTheSame_EvaluatesTrue()
    {
        //Arrange
        var password = "SomePassword@123";
        var externalIdentity = new CateringIdentity(
            "Some Email",
            new FullName("Test", "Test"),
            password,
            IdentityRoleExtensions.GetRestaurantEmployee());

        //Act
        var result = externalIdentity.ComparePassword(password);

        //Assert
        Assert.True(result);
    }

    [Theory]
    [InlineData("")]
    [InlineData(" ")]
    public void EditPassword_PasswordIsInvalid_ArgumentException(string invalidPassword)
    {
        //Arrange
        var externalIdentity = new CateringIdentity(
            "Some Email",
            new FullName("Test", "Test"),
            "SomePassword@123",
            IdentityRoleExtensions.GetRestaurantEmployee());

        //Act
        void a() => externalIdentity.EditPassword(invalidPassword);

        //Assert
        Assert.Throws<ArgumentException>(a);
    }

    [Fact]
    public void EditPassword_PasswordIsNull_ArgumentNullException()
    {
        //Arrange
        var externalIdentity = new CateringIdentity(
            "Some Email",
            new FullName("Test", "Test"),
            "SomePassword@123",
            IdentityRoleExtensions.GetRestaurantEmployee());

        //Act
        void a() => externalIdentity.EditPassword(null);

        //Assert
        Assert.Throws<ArgumentNullException>(a);
    }

    [Fact]
    public void EditPassword_ValidPasswordPassed_PasswordChanged()
    {
        //Arrange
        var newPassword = "NewPassword@123";
        var externalIdentity = new CateringIdentity(
            "Some Email",
            new FullName("Test", "Test"),
            "SomePassword@123",
            IdentityRoleExtensions.GetRestaurantEmployee());

        //Act
        externalIdentity.EditPassword(newPassword);

        //Assert
        Assert.True(externalIdentity.ComparePassword(newPassword));
    }
}
