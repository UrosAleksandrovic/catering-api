using Catering.Domain.Entities.IdentityAggregate;
using Catering.Domain.Exceptions;
using System;
using System.Collections.Generic;
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
            new FullName("Test", "Test"),
            "SomeEmail",
            IdentityRole.RestourantEmployee);

        //Act
        void a() => identity.Edit(invalidEmail, new FullName("Test", "Test"));

        //Assert
        Assert.Throws<ArgumentException>(a);
    }

    [Fact]
    public void Edit_EmailIsNull_ArgumentNullException()
    {
        //Arrange
        var identity = new Identity(
            new FullName("Test", "Test"),
            "SomeEmail",
            IdentityRole.RestourantEmployee);

        //Act
        void a() => identity.Edit(null, new FullName("Test", "Test"));

        //Assert
        Assert.Throws<ArgumentNullException>(a);
    }

    [Fact]
    public void Edit_ChangeEmailAndFullName_PropertiesAreChanged()
    {
        //Arrange
        var expectedValue = "New Value";
        var identity = new Identity(
            new FullName("Test", "Test"),
            "SomeEmail",
            IdentityRole.RestourantEmployee);

        //Act
        identity.Edit(expectedValue, new FullName(expectedValue, expectedValue));

        //Assert
        Assert.Equal(expectedValue, identity.Email);
        Assert.Equal(expectedValue, identity.FullName.FirstName);
        Assert.Equal(expectedValue, identity.FullName.LastName);
    }

    [Fact]
    public void EditOtherIdentity_IdentityIsNull_ArgumentNullException()
    {
        //Arrange
        var expectedValue = "New Value";

        var admin = new Identity(
            new FullName("Test", "Test"),
            "SomeEmail",
            IdentityRole.SuperAdministrator);

        //Act
        void a() => admin.EditOtherIdentity(
            null,
            expectedValue,
            new FullName(expectedValue, expectedValue),
            new[] { IdentityRole.RestourantEmployee });

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
            new FullName("Test", "Test"),
            "SomeEmail",
            IdentityRole.SuperAdministrator);

        var identity = new Identity(
            new FullName("Test", "Test"),
            "SomeEmail",
            IdentityRole.RestourantEmployee);

        //Act
        void a() => admin.EditOtherIdentity(
            identity,
            invalidEmail,
            new FullName(expectedValue, expectedValue),
            new[] { IdentityRole.CompanyAdministrator });

        //Assert
        Assert.Throws<ArgumentException>(a);
    }

    [Fact]
    public void EditOtherIdentity_EmailIsNull_ArgumentNullException()
    {
        //Arrange
        var expectedValue = "New Value";

        var admin = new Identity(
            new FullName("Test", "Test"),
            "SomeEmail",
            IdentityRole.CompanyAdministrator);

        var identity = new Identity(
            new FullName(expectedValue, expectedValue),
            "SomeEmail",
            IdentityRole.RestourantEmployee);

        //Act
        void a() => admin.EditOtherIdentity(
            identity,
            null,
            new FullName(expectedValue, expectedValue),
            new[] { IdentityRole.CompanyAdministrator });

        //Assert
        Assert.Throws<ArgumentNullException>(a);
    }

    [Fact]
    public void EditOtherIdentity_InitiatorIsNotAdministrator_ArgumentNullException()
    {
        //Arrange
        var expectedValue = "New Value";

        var admin = new Identity(
            new FullName("Test", "Test"),
            "SomeEmail",
            IdentityRole.CompanyEmployee);

        var identity = new Identity(
            new FullName("Test", "Test"),
            "SomeEmail",
            IdentityRole.RestourantEmployee);

        //Act
        void a() => admin.EditOtherIdentity(
            identity,
            expectedValue,
            new FullName(expectedValue, expectedValue),
            new[] { IdentityRole.CompanyAdministrator });

        //Assert
        Assert.Throws<IdentityRestrictionException>(a);
    }

    [Fact]
    public void EditOtherIdentity_PassEmptyRoles_ArgumentException()
    {
        //Arrange
        var expectedValue = "New Value";

        var admin = new Identity(
            new FullName("Test", "Test"),
            "SomeEmail",
            IdentityRole.SuperAdministrator);

        var identity = new Identity(
            new FullName("Test", "Test"),
            "SomeEmail",
            IdentityRole.RestourantEmployee);

        //Act
        void a() => admin.EditOtherIdentity(
            identity,
            expectedValue,
            new FullName(expectedValue, expectedValue),
            new List<string>());

        //Assert
        Assert.Throws<ArgumentException>(a);
    }

    [Fact]
    public void EditOtherIdentity_PassNullRoles_ArgumentNullException()
    {
        //Arrange
        var expectedValue = "New Value";

        var admin = new Identity(
            new FullName("Test", "Test"),
            "SomeEmail",
            IdentityRole.SuperAdministrator);

        var identity = new Identity(
            new FullName("Test", "Test"),
            "SomeEmail",
            IdentityRole.RestourantEmployee);

        //Act
        void a() => admin.EditOtherIdentity(
            identity,
            expectedValue,
            new FullName(expectedValue, expectedValue),
            null);

        //Assert
        Assert.Throws<ArgumentNullException>(a);
    }

    [Fact]
    public void EditOtherIdentity_ChangeEverything_PropertiesChanged()
    {
        //Arrange
        var expectedValue = "New Value";

        var admin = new Identity(
            new FullName("Test", "Test"),
            "SomeEmail",
            IdentityRole.CompanyAdministrator);

        var identity = new Identity(
            new FullName("Test", "Test"),
            "SomeEmail",
            IdentityRole.RestourantEmployee);

        //Act
        admin.EditOtherIdentity(
            identity,
            expectedValue,
            new FullName(expectedValue, expectedValue),
            new[] { IdentityRole.CompanyAdministrator });

        //Assert
        Assert.Equal(expectedValue, identity.FullName.FirstName);
        Assert.Equal(expectedValue, identity.FullName.LastName);
        Assert.Equal(expectedValue, identity.Email);
        Assert.Equal(1, identity.Roles.Count);
        Assert.Contains(IdentityRole.CompanyAdministrator, identity.Roles);
    }
}
