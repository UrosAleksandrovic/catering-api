using Catering.Domain.Entities.UserAggregate;
using Catering.Domain.Exceptions;
using System;
using Xunit;

namespace Catering.Domain.Test.UserAggregate;

public class UserTest
{
    [Fact]
    public void IsCompanyEmployee_RestourantEmployeeRole_ReturnsFalse()
    {
        //Arrange
        var user = new User(Guid.NewGuid().ToString(), "SomeEmail", true);

        //Assert
        Assert.False(user.IsComapnyEmployee);
    }

    [Fact]
    public void IsCompanyEmployee_HasEmployeeRole_ReturnsTrue()
    {
        //Arrange
        var user = new User(Guid.NewGuid().ToString(), "SomeEmail", false);

        //Assert
        Assert.True(user.IsComapnyEmployee);
    }

    [Fact]
    public void ResetBudget_UserIsRestourantEmployee_NotAllowedActionException()
    {
        //Arrange
        var user = new User(Guid.NewGuid().ToString(), "Some email", true);

        //Act
        void a() => user.ResetBudget(100);

        //Assert
        Assert.Throws<RestourantNotAllowedActionException>(a);
    }

    [Fact]
    public void ResetBudget_ValidPath_BudgetBalanceSetToNewValue()
    {
        //Arrange
        var newBudget = 100;
        var user = new User(Guid.NewGuid().ToString(), "Some email", false);

        //Act
        user.ResetBudget(newBudget);

        //Assert
        Assert.Equal(newBudget, user.Budget.Balance);
    }

    [Fact]
    public void ReserveAssets_UserIsResourantEmployee_NotAllowedActionException()
    {
        //Arrange
        var user = new User(Guid.NewGuid().ToString(), "Some email", true);

        //Act
        void a() => user.ReserveAssets(100);

        //Assert
        Assert.Throws<RestourantNotAllowedActionException>(a);
    }

    [Fact]
    public void ReserveAssets_ValidPath_BudgetBalanceSetToNewValue()
    {
        //Arrange
        var reservedBalance = 100;
        var user = new User(Guid.NewGuid().ToString(), "Some email", false);
        user.ResetBudget(reservedBalance + 1);

        //Act
        user.ReserveAssets(reservedBalance);

        //Assert
        Assert.Equal(reservedBalance, user.Budget.ReservedAssets);
        Assert.Equal(reservedBalance + 1, user.Budget.Balance);
    }

    [Fact]
    public void CancelPayment_UserIsResourantEmployee_NotAllowedActionException()
    {
        //Arrange
        var user = new User(Guid.NewGuid().ToString(), "Some email", true);

        //Act
        void a() => user.CancelPayment(100);

        //Assert
        Assert.Throws<RestourantNotAllowedActionException>(a);
    }

    [Fact]
    public void CancelPayment_ValidPath_BudgetBalanceSetToNewValue()
    {
        //Arrange
        var reservedBalance = 100;
        var user = new User(Guid.NewGuid().ToString(), "Some email", false);
        user.ResetBudget(reservedBalance + 1);
        user.ReserveAssets(reservedBalance);

        //Act
        user.CancelPayment(reservedBalance);

        //Assert
        Assert.Equal(0, user.Budget.ReservedAssets);
        Assert.Equal(reservedBalance + 1, user.Budget.Balance);
    }
}
