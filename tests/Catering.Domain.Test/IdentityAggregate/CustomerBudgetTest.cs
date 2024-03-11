using System;
using Catering.Domain.Aggregates.Identity;
using Xunit;

namespace Catering.Domain.Test.IdentityAggregate;

public class CustomerBudgetTest
{
    [Fact]
    public void SetBalance_NegativeBalance_Exception()
    {
        //Arrange
        var budget = new CustomerBudget(100);

        //Assert
        void a() => budget.SetBalance(-1);

        //Act
        Assert.Throws<ArgumentException>(a);
    }

    [Fact]
    public void SetBalance_ValidPath_BalanceChanged()
    {
        //Arrange
        var newAmount = 200;
        var budget = new CustomerBudget(100);

        //Assert
        budget.SetBalance(newAmount);

        //Act
        Assert.Equal(newAmount, budget.Balance);
    }

    [Fact]
    public void Reserve_NegativeAmountPassed_Expception()
    {
        //Arrange
        var budget = new CustomerBudget(100);

        //Assert
        void a() => budget.Reserve(-100);

        //Act
        Assert.Throws<ArgumentException>(a);
    }

    [Fact]
    public void Reserve_ValidPath_ReservedAssetsIncreased()
    {
        //Arrange
        var reservedBudget = 50;
        var initialBudget = 100;
        var budget = new CustomerBudget(initialBudget);

        //Assert
        budget.Reserve(reservedBudget);
        budget.Reserve(reservedBudget);

        //Act
        Assert.Equal(reservedBudget * 2, budget.ReservedAssets);
        Assert.Equal(initialBudget, budget.Balance);
    }

    [Fact]
    public void CancelReservation_AmountNegative_Exception()
    {
        //Arrange
        var budget = new CustomerBudget(100);

        //Assert
        void a() => budget.CancelReservation(-100);

        //Act
        Assert.Throws<ArgumentException>(a);
    }

    [Fact]
    public void CancelReservation_AmountBiggerThenReservedAssets_ArgumentOutOfRangeException()
    {
        //Arrange
        var reservedBudget = 50;
        var initialBudget = 100;
        var budget = new CustomerBudget(initialBudget);
        budget.Reserve(reservedBudget);

        //Assert
        void a() => budget.CancelReservation(reservedBudget + 1);

        //Act
        Assert.Throws<ArgumentOutOfRangeException>(a);
    }

    [Fact]
    public void CancelReservation_ValidPath_ReservedAssetsDecreased()
    {
        //Arrange
        var reservedBudget = 50;
        var initialBudget = 100;
        var budget = new CustomerBudget(initialBudget);
        budget.Reserve(reservedBudget);
        budget.Reserve(reservedBudget);

        //Assert
        budget.CancelReservation(reservedBudget);

        //Act
        Assert.Equal(reservedBudget, budget.ReservedAssets);
        Assert.Equal(initialBudget, budget.Balance);
    }

    [Fact]
    public void Remove_NegativeAmountPassed_Exception()
    {
        //Arrange
        var budget = new CustomerBudget(100);

        //Assert
        void a() => budget.Remove(-100);

        //Act
        Assert.Throws<ArgumentException>(a);
    }

    [Fact]
    public void Remove_AmountBiggerThenReserved_ArgumentOutOfRangeException()
    {
        //Arrange
        var reservedBudget = 50;
        var initialBudget = 100;
        var budget = new CustomerBudget(initialBudget);
        budget.Reserve(reservedBudget);

        //Assert
        void a() => budget.Remove(reservedBudget + 1);

        //Act
        Assert.Throws<ArgumentOutOfRangeException>(a);
    }

    [Fact]
    public void Remove_ValidPath_PaymentProcessed()
    {
        //Arrange
        var reservedBudget = 50;
        var initialBudget = 100;
        var budget = new CustomerBudget(initialBudget);
        budget.Reserve(reservedBudget);
        budget.Reserve(reservedBudget);

        //Assert
        budget.Remove(reservedBudget + 1);

        //Act
        Assert.Equal(reservedBudget - 1, budget.ReservedAssets);
        Assert.Equal(initialBudget - reservedBudget - 1, budget.Balance);
    }
}
