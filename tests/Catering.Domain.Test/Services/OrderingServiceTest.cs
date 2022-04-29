using Catering.Domain.Builders;
using Catering.Domain.Entities.IdentityAggregate;
using Catering.Domain.Entities.OrderAggregate;
using Catering.Domain.Services;
using FakeItEasy;
using System;
using Xunit;

namespace Catering.Domain.Test.Services;

public class OrderingServiceTest
{
    [Fact]
    public void PlaceOrder_CustomerIsNull_ArgumentNullException()
    {
        //Arrange
        var orderingService = new OrderingService();

        //Act
        void a() => orderingService.PlaceOrder(null, new OrderBuilder());

        //Assert
        Assert.Throws<ArgumentException>(a);
    }

    [Fact]
    public void PlaceOrder_OrderBuilderIsNull_ArgumentNullException()
    {
        //Arrange
        var orderingService = new OrderingService();
        var customer = new Customer(
            new FullName("Test", "Test"),
            "Some email",
            IdentityRole.CompanyEmployee);

        //Act
        void a() => orderingService.PlaceOrder(customer, null);

        //Assert
        Assert.Throws<ArgumentException>(a);
    }

    [Fact]
    public void PlaceOrder_ValidPath_OrderCreatedCustomerBudgetReserved()
    {
        //Arrange
        var orderingService = new OrderingService();
        var customer = new Customer(
            new FullName("Test", "Test"),
            "Some email",
            IdentityRole.CompanyEmployee);
        var orderBuilder = A.Fake<IBuilder<Order>>();
        A.CallTo(() => orderBuilder.Build()).Returns(
            new Order(new[]
            {
                new OrderItem(Guid.NewGuid(), 1, 1, null)
            },
            Guid.NewGuid(),
            "someid",
            DateTime.Now));

        //Act
        var resultOrder = orderingService.PlaceOrder(customer, orderBuilder);

        //Assert
        Assert.Equal(OrderStatus.Subbmited, resultOrder.Status);
        Assert.Equal(resultOrder.TotalPrice, customer.Budget.ReservedAssets);
    }

    [Fact]
    public void ConfirmOrder_CustomerIsNull_ArgumentNullException()
    {
        //Arrange
        var orderingService = new OrderingService();

        //Act
        void a() => orderingService.ConfirmOrder(null, null);

        //Assert
        Assert.Throws<ArgumentException>(a);
    }

    [Fact]
    public void ConfirmOrder_OrderIsNull_ArgumentNullException()
    {
        //Arrange
        var orderingService = new OrderingService();
        var customer = new Customer(
            new FullName("Test", "Test"),
            "Some email",
            IdentityRole.CompanyEmployee);

        //Act
        void a() => orderingService.ConfirmOrder(customer, null);

        //Assert
        Assert.Throws<ArgumentException>(a);
    }

    [Fact]
    public void ConfirmOrder_ValidPath_ReservedAssetsAreProcessed()
    {
        //Arrange
        var itemPrice = 100;
        var orderingService = new OrderingService();
        var customer = new Customer(
            new FullName("Test", "Test"),
            "Some email",
            IdentityRole.CompanyEmployee);
        customer.ResetBudget(100);
        var orderBuilder = A.Fake<IBuilder<Order>>();
        A.CallTo(() => orderBuilder.Build()).Returns(
            new Order(new[]
            {
                new OrderItem(Guid.NewGuid(), itemPrice, 1, null)
            },
            Guid.NewGuid(),
            "someid",
            DateTime.Now));
        var order = orderingService.PlaceOrder(customer, orderBuilder);

        //Act
        orderingService.ConfirmOrder(customer, order);

        //Assert
        Assert.Equal(OrderStatus.Confirmed, order.Status);
        Assert.Equal(0, customer.Budget.ReservedAssets);
        Assert.Equal(0, customer.Budget.Balance);
    }

    [Fact]
    public void CancelOrder_CustomerIsNull_ArgumentNullException()
    {
        //Arrange
        var orderingService = new OrderingService();

        //Act
        void a() => orderingService.CancelOrder(null, null);

        //Assert
        Assert.Throws<ArgumentException>(a);
    }

    [Fact]
    public void CancelOrder_OrderIsNull_ArgumentNullException()
    {
        //Arrange
        var orderingService = new OrderingService();
        var customer = new Customer(
            new FullName("Test", "Test"),
            "Some email",
            IdentityRole.CompanyEmployee);

        //Act
        void a() => orderingService.CancelOrder(customer, null);

        //Assert
        Assert.Throws<ArgumentException>(a);
    }

    [Fact]
    public void CancelOrder_ValidPath_OrderCanceledCustomerIsRestored()
    {

        //Arrange
        var itemPrice = 100;
        var orderingService = new OrderingService();
        var customer = new Customer(
            new FullName("Test", "Test"),
            "Some email",
            IdentityRole.CompanyEmployee);
        customer.ResetBudget(100);
        var orderBuilder = A.Fake<IBuilder<Order>>();
        A.CallTo(() => orderBuilder.Build()).Returns<Order>(
            new Order(new[]
            {
                new OrderItem(Guid.NewGuid(), itemPrice, 1, null)
            },
            Guid.NewGuid(),
            "someid",
            DateTime.Now));
        var order = orderingService.PlaceOrder(customer, orderBuilder);

        //Act
        orderingService.CancelOrder(customer, order);

        //Assert
        Assert.Equal(OrderStatus.Canceled, order.Status);
        Assert.Equal(0, customer.Budget.ReservedAssets);
        Assert.Equal(100, customer.Budget.Balance);
    }
}
