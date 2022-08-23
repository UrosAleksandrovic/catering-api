using Catering.Domain.Entities.IdentityAggregate;
using Xunit;

namespace Catering.Domain.Test.CustomerAggregate;

public class CustomerTest
{

    [Fact]
    public void ResetBudget_NewBalancePassed_BudgetBalanceSetToNewValue()
    {
        //Arrange
        var newBudget = 100;
        var identity = new Identity(new FullName("Test", "Test"),
            "test@test.com", IdentityRoleExtensions.GetClientAdministrator());
        var customer = new Customer(identity);

        //Act
        customer.ResetBudget(newBudget);

        //Assert
        Assert.Equal(newBudget, customer.Budget.Balance);
    }

    [Fact]
    public void ReserveAssets_ReserveAssets_BudgetBalanceSetToNewValue()
    {
        //Arrange
        var reservedBalance = 100;
        var identity = new Identity(new FullName("Test", "Test"),
            "test@test.com", IdentityRoleExtensions.GetClientAdministrator());
        var customer = new Customer(identity);
        customer.ResetBudget(reservedBalance + 1);

        //Act
        customer.ReserveAssets(reservedBalance);

        //Assert
        Assert.Equal(reservedBalance, customer.Budget.ReservedAssets);
        Assert.Equal(reservedBalance + 1, customer.Budget.Balance);
    }

    [Fact]
    public void CancelPayment_CancelPaymentOfAmount_BudgetBalanceSetToNewValue()
    {
        //Arrange
        var reservedBalance = 100;
        var identity = new Identity(new FullName("Test", "Test"), 
            "test@test.com", IdentityRoleExtensions.GetClientAdministrator());
        var customer = new Customer(identity);
        customer.ResetBudget(reservedBalance + 1);
        customer.ReserveAssets(reservedBalance);

        //Act
        customer.CancelPayment(reservedBalance);

        //Assert
        Assert.Equal(0, customer.Budget.ReservedAssets);
        Assert.Equal(reservedBalance + 1, customer.Budget.Balance);
    }

    [Fact]
    public void ProcessPayment_ProcessPaymentOfAmount_BudgetBalanceSetToNewValue()
    {
        //Arrange
        var reservedBalance = 100;
        var identity = new Identity(new FullName("Test", "Test"),
            "test@test.com", IdentityRoleExtensions.GetClientAdministrator());
        var customer = new Customer(identity);
        customer.ResetBudget(2 * reservedBalance + 1);
        customer.ReserveAssets(reservedBalance);
        customer.ReserveAssets(reservedBalance);

        //Act
        customer.ProcessPayment(reservedBalance);

        //Assert
        Assert.Equal(reservedBalance, customer.Budget.ReservedAssets);
        Assert.Equal(reservedBalance + 1, customer.Budget.Balance);
    }
}
