using Catering.Domain.Builders;
using Catering.Domain.Entities.OrderAggregate;
using Catering.Domain.Entities.UserAggregate;
using Catering.Domain.Services;
using FakeItEasy;
using System;
using Xunit;

namespace Catering.Domain.Test.Services;

public class OrderingServiceTest
{
    [Fact]
    public void PlaceOrder_UserIsNull_ArgumentNullException()
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
        var user = new User(Guid.NewGuid().ToString(), "Some email", false);

        //Act
        void a() => orderingService.PlaceOrder(user, null);

        //Assert
        Assert.Throws<ArgumentException>(a);
    }

    [Fact]
    public void PlaceOrder_ValidPath_OrderCreatedUserBudgetReserved()
    {
        //Arrange
        var orderingService = new OrderingService();
        var user = new User(Guid.NewGuid().ToString(), "Some email", false);
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
        var resultOrder = orderingService.PlaceOrder(user, orderBuilder);

        //Assert
        Assert.Equal(OrderStatus.Subbmited, resultOrder.Status);
        Assert.Equal(resultOrder.TotalPrice, user.Budget.ReservedAssets);
    }

    [Fact]
    public void ConfirmOrder_UserIsNull_ArgumentNullException()
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
        var user = new User(Guid.NewGuid().ToString(), "Some email", false);

        //Act
        void a() => orderingService.ConfirmOrder(user, null);

        //Assert
        Assert.Throws<ArgumentException>(a);
    }

    [Fact]
    public void ConfirmOrder_ValidPath_ReservedAssetsAreProcessed()
    {
        //Arrange
        var itemPrice = 100;
        var orderingService = new OrderingService();
        var user = new User(Guid.NewGuid().ToString(), "Some email", false);
        user.ResetBudget(100);
        var orderBuilder = A.Fake<IBuilder<Order>>();
        A.CallTo(() => orderBuilder.Build()).Returns(
            new Order(new[]
            {
                new OrderItem(Guid.NewGuid(), itemPrice, 1, null)
            },
            Guid.NewGuid(),
            "someid",
            DateTime.Now));
        var order = orderingService.PlaceOrder(user, orderBuilder);

        //Act
        orderingService.ConfirmOrder(user, order);

        //Assert
        Assert.Equal(OrderStatus.Confirmed, order.Status);
        Assert.Equal(0, user.Budget.ReservedAssets);
        Assert.Equal(0, user.Budget.Balance);
    }

    [Fact]
    public void CancelOrder_UserIsNull_ArgumentNullException()
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
        var user = new User(Guid.NewGuid().ToString(), "Some email", false);

        //Act
        void a() => orderingService.CancelOrder(user, null);

        //Assert
        Assert.Throws<ArgumentException>(a);
    }

    [Fact]
    public void CancelOrder_ValidPath_OrderCanceledUserIsRestored()
    {

        //Arrange
        var itemPrice = 100;
        var orderingService = new OrderingService();
        var user = new User(Guid.NewGuid().ToString(), "Some email", false);
        user.ResetBudget(100);
        var orderBuilder = A.Fake<IBuilder<Order>>();
        A.CallTo(() => orderBuilder.Build()).Returns<Order>(
            new Order(new[]
            {
                new OrderItem(Guid.NewGuid(), itemPrice, 1, null)
            },
            Guid.NewGuid(),
            "someid",
            DateTime.Now));
        var order = orderingService.PlaceOrder(user, orderBuilder);

        //Act
        orderingService.CancelOrder(user, order);

        //Assert
        Assert.Equal(OrderStatus.Canceled, order.Status);
        Assert.Equal(0, user.Budget.ReservedAssets);
        Assert.Equal(100, user.Budget.Balance);
    }
}
