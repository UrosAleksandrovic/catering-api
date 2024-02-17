using System;
using System.Linq;
using Catering.Domain.Aggregates.Cart;
using Catering.Domain.Exceptions;
using Xunit;

namespace Catering.Domain.Test.CartAggregate;

public class CartTest
{
    [Fact]
    public void AddItem_DifferentMenuIdPassed_ItemMenuNotValidException()
    {
        //Arrange
        var alreadyExistingItemId = Guid.NewGuid();
        var cart = new Cart(Guid.NewGuid().ToString());
        cart.AddItem(Guid.NewGuid(), alreadyExistingItemId, 1);

        //Act
        void a() => cart.AddItem(Guid.NewGuid(), alreadyExistingItemId, 2);

        //Assert
        Assert.Throws<ItemMenuNotValidException>(a);
    }

    [Fact]
    public void AddItem_ItemAlreadyExistsInCart_ItemAlreadyInCartException()
    {
        //Arrange
        var alreadyExistingItemId = Guid.NewGuid();
        var alreadyExistingMenuId = Guid.NewGuid();
        var cart = new Cart(Guid.NewGuid().ToString());
        cart.AddItem(alreadyExistingMenuId, alreadyExistingItemId, 1);

        //Act
        void a() => cart.AddItem(alreadyExistingMenuId, alreadyExistingItemId, 2);

        //Assert
        Assert.Throws<ItemAlreadyInCartException>(a);
    }

    [Fact]
    public void AddItem_ItemDoesNotExist_NewItemIsAdded()
    {
        //Arrange
        var expectedItemId = Guid.NewGuid();
        var expectedMenuId = Guid.NewGuid();
        var expectedQuantityAfterAddition = 5;
        var expectedNote = "Test note";
        var cart = new Cart(Guid.NewGuid().ToString());
        cart.AddItem(expectedMenuId, Guid.NewGuid(), 1);

        //Act
        cart.AddItem(expectedMenuId, expectedItemId, expectedQuantityAfterAddition, expectedNote);

        //Assert
        Assert.Equal(2, cart.Items.Count);
        Assert.Equal(expectedMenuId, cart.MenuId);
        Assert.Contains(
            cart.Items,
            i => i.ItemId == expectedItemId 
                && i.Quantity == expectedQuantityAfterAddition
                && i.Note == expectedNote);
    }

    [Fact]
    public void IncrementItem_ItemIsNotInCart_ItemNotInCartException()
    {
        //Arrange
        var cart = new Cart(Guid.NewGuid().ToString());

        //Act
        void a() => cart.IncrementItem(Guid.NewGuid());

        //Assert
        Assert.Throws<ItemNotInCartException>(a);
    }

    [Theory]
    [InlineData(1)]
    [InlineData(2)]
    public void IncrementItem_ItemInCart_ItemQuantityIncremented(int incrementValue)
    {
        //Arrange
        var cart = new Cart(Guid.NewGuid().ToString());
        var itemId = Guid.NewGuid();
        var menuId = Guid.NewGuid();
        var itemQuantity = 2;
        cart.AddItem(menuId, itemId, itemQuantity);

        //Act
        cart.IncrementItem(itemId, incrementValue);

        //Assert
        Assert.Equal(
            itemQuantity + incrementValue,
            cart.Items.Single(i => i.ItemId == itemId).Quantity);
    }

    [Fact]
    public void DecrementOrDeleteItem_ItemIsNotInCart_ItemNotInCartException()
    {
        //Arrange
        var cart = new Cart(Guid.NewGuid().ToString());

        //Act
        void a() => cart.DecrementOrDeleteItem(Guid.NewGuid());

        //Assert
        Assert.Throws<ItemNotInCartException>(a);
    }

    [Theory]
    [InlineData(1)]
    [InlineData(2)]
    public void DecrementOrDeleteItem_QuantityLessThenItemQuantity_ItemQuantityDecreased(int value)
    {
        //Arrange
        var cart = new Cart(Guid.NewGuid().ToString());
        var itemId = Guid.NewGuid();
        var menuId = Guid.NewGuid();
        var itemQuantity = 10;
        cart.AddItem(menuId, itemId, itemQuantity);

        //Act
        cart.DecrementOrDeleteItem(itemId, value);

        //Assert
        Assert.Equal(
            itemQuantity - value,
            cart.Items.Single(i => i.ItemId == itemId).Quantity);
    }

    [Theory]
    [InlineData(3)]
    [InlineData(100)]
    [InlineData(int.MaxValue)]
    public void DecrementOrDeleteItem_QuantityGreaterThenItemQuantity_ItemRemoved(int value)
    {
        //Arrange
        var cart = new Cart(Guid.NewGuid().ToString());
        var itemId = Guid.NewGuid();
        var menuId = Guid.NewGuid();
        var itemQuantity = 3;
        cart.AddItem(menuId, itemId, itemQuantity);

        //Act
        cart.DecrementOrDeleteItem(itemId, value);

        //Assert
        Assert.DoesNotContain(cart.Items, i => i.ItemId == itemId);
        Assert.Null(cart.MenuId);
    }

    [Fact]
    public void AddOrEditNoteToItem_ItemNotInCart_ItemNotInCartException()
    {
        //Arrange
        var cart = new Cart(Guid.NewGuid().ToString());

        //Act
        void a() => cart.AddOrEditNoteToItem(Guid.NewGuid(), "Some Note");

        //Assert
        Assert.Throws<ItemNotInCartException>(a);
    }

    [Theory]
    [InlineData(null)]
    [InlineData(" ")]
    [InlineData("")]
    [InlineData("Initial Note")]
    public void AddOrEditNoteToItem_ItemInCart_NoteAdded(string initialNote)
    {
        //Arrange
        var cart = new Cart(Guid.NewGuid().ToString());
        var itemId = Guid.NewGuid();
        var menuId = Guid.NewGuid();
        var newNote = "Some new Note";
        cart.AddItem(menuId, itemId, 1, initialNote);

        //Act
        cart.AddOrEditNoteToItem(itemId, newNote);

        //Assert
        Assert.Equal(newNote, cart.Items.Single(i => i.ItemId == itemId).Note);
    }
}
