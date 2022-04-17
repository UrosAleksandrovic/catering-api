using Catering.Domain.Builders;
using Catering.Domain.Entities.CartAggregate;
using Catering.Domain.Entities.ItemAggregate;
using Catering.Domain.Entities.OrderAggregate;
using Catering.Domain.Exceptions;
using System;
using Xunit;

namespace Catering.Domain.Test.OrderAggregate;

public class OrderTest
{
    private readonly OrderBuilder _orderBuilder;

    public OrderTest()
    {
        _orderBuilder = InitDefaultOrderBuidler();
    }

    private OrderBuilder InitDefaultOrderBuidler()
    {
        var menuId = Guid.NewGuid();
        var items = new[]
        {
            new Item("Chocolate", "Some description", 10, menuId),
        };
        var cart = new Cart(Guid.NewGuid().ToString());
        cart.AddItem(items[0].Id, 1);

        return new OrderBuilder()
            .HasDateOfDelivery(DateTime.Today)
            .HasItems(items)
            .HasCart(cart);
    }

    [Fact]
    public void IsForHomeDelivery_HomeDeliveryNull_FalseReturned()
    {
        //Arrange
        var order = _orderBuilder.Build();

        //Assert
        Assert.Null(order.HomeDeliveryInfo);
        Assert.False(order.IsForHomeDelivery);
    }

    [Fact]
    public void IsForHomeDelivery_HomeDeliveryNotNull_TrueReturned()
    {
        //Arrange
        var order = _orderBuilder
            .HasHomeDeliveryOption("Street", "4th/400")
            .Build();

        //Assert
        Assert.NotNull(order.HomeDeliveryInfo);
        Assert.True(order.IsForHomeDelivery);
    }

    [Fact]
    public void ConfirmOrder_OrderIsAlreadyConfirmed_WrongOrderStatusException()
    {
        //Arrange
        var order = _orderBuilder.Build();
        order.ConfirmOrder();

        //Act
        void a() => order.ConfirmOrder();

        //Assert
        Assert.Equal(OrderStatus.Confirmed, order.Status);
        Assert.Throws<WrongOrderStatusException>(a);
    }

    [Fact]
    public void ConfirmOrder_OrderIsAlreadyCancelled_WrongOrderStatusException()
    {
        //Arrange
        var order = _orderBuilder.Build();
        order.CancelOrder();

        //Act
        void a() => order.ConfirmOrder();

        //Assert
        Assert.Equal(OrderStatus.Canceled, order.Status);
        Assert.Throws<WrongOrderStatusException>(a);
    }

    [Fact]
    public void ConfirmOrder_OrderIsSubbmited_OrderStatusChanged()
    {
        //Arrange
        var order = _orderBuilder.Build();

        //Act
        order.ConfirmOrder();

        //Assert
        Assert.Equal(OrderStatus.Confirmed, order.Status);
    }

    [Fact]
    public void CancelOrder_OrderIsAlreadyConfirmed_WrongOrderStatusException()
    {
        //Arrange
        var order = _orderBuilder.Build();
        order.ConfirmOrder();

        //Act
        void a() => order.CancelOrder();

        //Assert
        Assert.Equal(OrderStatus.Confirmed, order.Status);
        Assert.Throws<WrongOrderStatusException>(a);
    }

    [Fact]
    public void CancelOrder_OrderIsAlreadyCancelled_WrongOrderStatusException()
    {
        //Arrange
        var order = _orderBuilder.Build();
        order.CancelOrder();

        //Act
        void a() => order.CancelOrder();

        //Assert
        Assert.Equal(OrderStatus.Canceled, order.Status);
        Assert.Throws<WrongOrderStatusException>(a);
    }

    [Fact]
    public void Cancel_OrderIsSubbmited_OrderStatusChanged()
    {
        //Arrange
        var order = _orderBuilder.Build();

        //Act
        order.CancelOrder();

        //Assert
        Assert.Equal(OrderStatus.Canceled, order.Status);
    }

    [Fact]
    public void TotalPrice_ValidPath_ReturnsSumOrItems()
    {
        //Arrange
        var itemPrice = 10;
        var itemQuantity = 2;
        var menuId = Guid.NewGuid();
        var items = new[]
        {
            new Item("Chocolate", "Some description", itemPrice, menuId),
            new Item("Bread", "Some description", itemPrice, menuId),
            new Item("Bananas", "Some description", itemPrice, menuId)
        };

        var cart = new Cart(Guid.NewGuid().ToString());
        foreach (var item in items)
            cart.AddItem(item.Id, itemQuantity);

        var order = _orderBuilder
            .HasCart(cart)
            .HasItems(items)
            .Build();

        //Assert
        Assert.Equal(itemPrice * items.Length * 2, order.TotalPrice);
    }
}
