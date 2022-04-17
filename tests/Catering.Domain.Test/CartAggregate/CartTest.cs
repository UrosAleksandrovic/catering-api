using Catering.Domain.Entities.CartAggregate;
using Catering.Domain.Exceptions;
using System;
using System.Linq;
using Xunit;

namespace Catering.Domain.Test.CartAggregate;

public class CartTest
{
    [Fact]
    public void AddItem_ItemAlreadyExistsInCart_ItemAlreadyInCartException()
    {
        //Arrange
        var alreadyExistingItemId = Guid.NewGuid();
        var cart = new Cart(Guid.NewGuid().ToString());
        cart.AddItem(alreadyExistingItemId, 1);

        //Act
        void a() => cart.AddItem(alreadyExistingItemId, 2);

        //Assert
        Assert.Throws<ItemAlreadyInCartException>(a);
    }

    [Fact]
    public void AddItem_ItemDoesNotExist_NewItemIsAdded()
    {
        //Arrange
        var expectedItemId = Guid.NewGuid();
        var expectedQuantityAfterAddition = 5;
        var expectedNote = "Test note";
        var cart = new Cart(Guid.NewGuid().ToString());
        cart.AddItem(Guid.NewGuid(), 1);

        //Act
        cart.AddItem(expectedItemId, expectedQuantityAfterAddition, expectedNote);

        //Assert
        Assert.Equal(2, cart.Items.Count);
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
        var itemQuantity = 2;
        cart.AddItem(itemId, itemQuantity);

        //Act
        cart.IncrementItem(itemId, incrementValue);

        //Assert
        Assert.Equal(
            itemQuantity + incrementValue,
            cart.Items.Single(i => i.ItemId == itemId).Quantity);
    }

    [Fact]
    public void DecrementItem_ItemIsNotInCart_ItemNotInCartException()
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
    public void DecrementItem_QuantityLessThenItemQuantity_ItemQuantityDecreased(int value)
    {
        //Arrange
        var cart = new Cart(Guid.NewGuid().ToString());
        var itemId = Guid.NewGuid();
        var itemQuantity = 10;
        cart.AddItem(itemId, itemQuantity);

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
    public void DecrementItem_QuantityGreaterThenItemQuantity_ItemRemoved(int value)
    {
        //Arrange
        var cart = new Cart(Guid.NewGuid().ToString());
        var itemId = Guid.NewGuid();
        var itemQuantity = 3;
        cart.AddItem(itemId, itemQuantity);

        //Act
        cart.DecrementOrDeleteItem(itemId, value);

        //Assert
        Assert.DoesNotContain(cart.Items, i => i.ItemId == itemId);
    }

    [Fact]
    public void AddNoteToItem_ItemNotInCart_ItemNotInCartException()
    {
        //Arrange
        var cart = new Cart(Guid.NewGuid().ToString());

        //Act
        void a() => cart.AddNoteToItem(Guid.NewGuid(), "Some Note");

        //Assert
        Assert.Throws<ItemNotInCartException>(a);
    }

    [Theory]
    [InlineData(null)]
    [InlineData(" ")]
    [InlineData("")]
    [InlineData("Initial Note")]
    public void AddNoteToItem_ItemInCart_NoteAdded(string initialNote)
    {
        //Arrange
        var cart = new Cart(Guid.NewGuid().ToString());
        var itemId = Guid.NewGuid();
        var newNote = "Some new Note";
        cart.AddItem(itemId, 1, initialNote);

        //Act
        cart.AddNoteToItem(itemId, newNote);

        //Assert
        Assert.Equal(newNote, cart.Items.Single(i => i.ItemId == itemId).Note);
    }
}
