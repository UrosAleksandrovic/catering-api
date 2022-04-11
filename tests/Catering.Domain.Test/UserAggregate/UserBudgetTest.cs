using Catering.Domain.Entities.UserAggregate;
using System;
using Xunit;

namespace Catering.Domain.Test.UserAggregate;

public class UserBudgetTest
{
    [Fact]
    public void SetBalance_NegativeBalance_Exception()
    {
        //Arrange
        var userBudget = new UserBudget(Guid.NewGuid().ToString(), 100);

        //Assert
        void a() => userBudget.SetBalance(-1);

        //Act
        Assert.Throws<ArgumentException>(a);
    }

    [Fact]
    public void SetBalance_ValidPath_BalanceChanged()
    {
        //Arrange
        var newAmount = 200;
        var userBudget = new UserBudget(Guid.NewGuid().ToString(), 100);

        //Assert
        userBudget.SetBalance(newAmount);

        //Act
        Assert.Equal(newAmount, userBudget.Balance);
    }

    [Fact]
    public void Reserve_NegativeAmountPassed_Expception()
    {
        //Arrange
        var userBudget = new UserBudget(Guid.NewGuid().ToString(), 100);

        //Assert
        void a() => userBudget.Reserve(-100);

        //Act
        Assert.Throws<ArgumentException>(a);
    }

    [Fact]
    public void Reserve_ValidPath_ReservedAssetsIncreased()
    {
        //Arrange
        var reservedBudget = 50;
        var initialBudget = 100;
        var userBudget = new UserBudget(Guid.NewGuid().ToString(), initialBudget);

        //Assert
        userBudget.Reserve(reservedBudget);
        userBudget.Reserve(reservedBudget);

        //Act
        Assert.Equal(reservedBudget * 2, userBudget.ReservedAssets);
        Assert.Equal(initialBudget, userBudget.Balance);
    }

    [Fact]
    public void CancelReservation_AmountNegative_Exception()
    {
        //Arrange
        var userBudget = new UserBudget(Guid.NewGuid().ToString(), 100);

        //Assert
        void a() => userBudget.CancelReservation(-100);

        //Act
        Assert.Throws<ArgumentException>(a);
    }

    [Fact]
    public void CancelReservation_AmountBiggerThenReservedAssets_ArgumentOutOfRangeException()
    {
        //Arrange
        var reservedBudget = 50;
        var initialBudget = 100;
        var userBudget = new UserBudget(Guid.NewGuid().ToString(), initialBudget);
        userBudget.Reserve(reservedBudget);

        //Assert
        void a() => userBudget.CancelReservation(reservedBudget + 1);

        //Act
        Assert.Throws<ArgumentOutOfRangeException>(a);
    }

    [Fact]
    public void CancelReservation_ValidPath_ReservedAssetsDecreased()
    {
        //Arrange
        var reservedBudget = 50;
        var initialBudget = 100;
        var userBudget = new UserBudget(Guid.NewGuid().ToString(), initialBudget);
        userBudget.Reserve(reservedBudget);
        userBudget.Reserve(reservedBudget);

        //Assert
        userBudget.CancelReservation(reservedBudget);

        //Act
        Assert.Equal(reservedBudget, userBudget.ReservedAssets);
        Assert.Equal(initialBudget, userBudget.Balance);
    }

    [Fact]
    public void Remove_NegativeAmountPassed_Exception()
    {
        //Arrange
        var userBudget = new UserBudget(Guid.NewGuid().ToString(), 100);

        //Assert
        void a() => userBudget.Remove(-100);

        //Act
        Assert.Throws<ArgumentException>(a);
    }

    [Fact]
    public void Remove_AmountBiggerThenReserved_ArgumentOutOfRangeException()
    {
        //Arrange
        var reservedBudget = 50;
        var initialBudget = 100;
        var userBudget = new UserBudget(Guid.NewGuid().ToString(), initialBudget);
        userBudget.Reserve(reservedBudget);

        //Assert
        void a() => userBudget.Remove(reservedBudget + 1);

        //Act
        Assert.Throws<ArgumentOutOfRangeException>(a);
    }

    [Fact]
    public void Remove_ValidPath_PaymentProcessed()
    {
        //Arrange
        var reservedBudget = 50;
        var initialBudget = 100;
        var userBudget = new UserBudget(Guid.NewGuid().ToString(), initialBudget);
        userBudget.Reserve(reservedBudget);
        userBudget.Reserve(reservedBudget);

        //Assert
        userBudget.Remove(reservedBudget + 1);

        //Act
        Assert.Equal(reservedBudget - 1, userBudget.ReservedAssets);
        Assert.Equal(initialBudget - reservedBudget - 1, userBudget.Balance);
    }
}
