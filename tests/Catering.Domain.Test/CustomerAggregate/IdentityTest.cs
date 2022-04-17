using Catering.Domain.Entities.IdentityAggregate;
using System;
using Xunit;

namespace Catering.Domain.Test.CustomerAggregate;

public class IdentityTest
{
    [Theory]
    [InlineData("")]
    [InlineData(" ")]
    public void Edit_InvalidEmail_ArgumentException(string invalidEmail)
    {
        //Arrange
        var identity = new Identity(
            "Full Name",
            "SomeEmail",
            IdentityPermissions.RestourantEmployee);

        //Act
        void a() => identity.Edit(invalidEmail, "Some full name");

        //Assert
        Assert.Throws<ArgumentException>(a);
    }

    [Fact]
    public void Edit_EmailIsNull_ArgumentNullException()
    {
        //Arrange
        var identity = new Identity(
            "Full Name",
            "SomeEmail",
            IdentityPermissions.RestourantEmployee);

        //Act
        void a() => identity.Edit(null, "Some full name");

        //Assert
        Assert.Throws<ArgumentNullException>(a);
    }

    [Fact]
    public void Edit_ChangeEmailAndFullName_PropertiesAreChanged()
    {
        //Arrange
        var expectedValue = "New Value";
        var identity = new Identity(
            "Full Name",
            "SomeEmail",
            IdentityPermissions.RestourantEmployee);

        //Act
        identity.Edit(expectedValue, expectedValue);

        //Assert
        Assert.Equal(expectedValue, identity.Email);
        Assert.Equal(expectedValue, identity.FullName);
    }

    [Fact]
    public void EditOtherIdentity_IdentityIsNull_ArgumentNullException()
    {
        //Arrange
        var expectedValue = "New Value";

        var admin = new Identity(
            "Full Name",
            "SomeEmail",
            IdentityPermissions.CompanyAdministrator);

        //Act
        void a() => admin.EditOtherIdentity(null, expectedValue, expectedValue, IdentityPermissions.CompanyAdministrator);

        //Assert
        Assert.Throws<ArgumentException>(a);
    }

    [Theory]
    [InlineData("")]
    [InlineData(" ")]
    public void EditOtherIdentity_EmailIsInvalid_ArgumentException(string invalidEmail)
    {
        //Arrange
        var expectedValue = "New Value";

        var admin = new Identity(
            "Full Name",
            "SomeEmail",
            IdentityPermissions.CompanyAdministrator);

        var identity = new Identity(
            "Full Name",
            "SomeEmail",
            IdentityPermissions.RestourantEmployee);

        //Act
        void a() => admin.EditOtherIdentity(identity, invalidEmail, expectedValue, IdentityPermissions.CompanyAdministrator);

        //Assert
        Assert.Throws<ArgumentException>(a);
    }

    [Fact]
    public void EditOtherIdentity_EmailIsNull_ArgumentNullException()
    {
        //Arrange
        var expectedValue = "New Value";

        var admin = new Identity(
            "Full Name",
            "SomeEmail",
            IdentityPermissions.CompanyAdministrator);

        var identity = new Identity(
            "Full Name",
            "SomeEmail",
            IdentityPermissions.RestourantEmployee);

        //Act
        void a() => admin.EditOtherIdentity(identity, null, expectedValue, IdentityPermissions.CompanyAdministrator);

        //Assert
        Assert.Throws<ArgumentNullException>(a);
    }

    [Fact]
    public void EditOtherIdentity_InitiatorIsNotAdministrator_ArgumentNullException()
    {
        //Arrange
        var expectedValue = "New Value";

        var admin = new Identity(
            "Full Name",
            "SomeEmail",
            IdentityPermissions.CompanyEmployee);

        var identity = new Identity(
            "Full Name",
            "SomeEmail",
            IdentityPermissions.RestourantEmployee);

        //Act
        void a() => admin.EditOtherIdentity(identity, expectedValue, expectedValue, IdentityPermissions.CompanyAdministrator);

        //Assert
        Assert.Throws<InvalidOperationException>(a);
    }

    public void EditOtherIdentity_ChangeEverything_PropertiesChanged()
    {
        //Arrange
        var expectedValue = "New Value";

        var admin = new Identity(
            "Full Name",
            "SomeEmail",
            IdentityPermissions.CompanyAdministrator);

        var identity = new Identity(
            "Full Name",
            "SomeEmail",
            IdentityPermissions.RestourantEmployee);

        //Act
        admin.EditOtherIdentity(identity, expectedValue, expectedValue, IdentityPermissions.CompanyAdministrator);

        //Assert
        Assert.Equal(expectedValue, identity.FullName);
        Assert.Equal(expectedValue, identity.Email);
        Assert.Equal(IdentityPermissions.CompanyAdministrator, identity.Permissions);

    }
}
