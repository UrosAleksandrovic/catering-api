using Catering.Domain.Entities.CartAggregate;
using System;
using Xunit;

namespace Catering.Domain.Test.CartAggregate;

public class CartItemTest
{
    [Theory]
    [InlineData(0)]
    [InlineData(-1)]
    [InlineData(int.MinValue)]
    public void IncrementQuantity_NegativeOrZeroPassed_ArgumentException(int quantity)
    {
        //Arrange
        var cartItem = new CartItem(Guid.NewGuid(), null);

        //Act
        void a() => cartItem.IncrementQuantity(quantity);

        //Assert
        Assert.Throws<ArgumentException>(a);
    }

    [Fact]
    public void IncrementQuantity_DefaultIncrement_IncrementedByOne()
    {
        //Arrange
        var startingQuantity = 2;
        var cartItem = new CartItem(Guid.NewGuid(), null, startingQuantity);

        //Act
        cartItem.IncrementQuantity();

        //Assert
        Assert.Equal(startingQuantity + 1, cartItem.Quantity);
    }

    [Theory]
    [InlineData(0)]
    [InlineData(-1)]
    [InlineData(int.MinValue)]
    public void DecrementQuantity_NegativeOrZeroPassed_ArgumentException(int quantity)
    {
        //Arrange
        var cartItem = new CartItem(Guid.NewGuid(), null);

        //Act
        void a() => cartItem.DecrementQuantity(quantity);

        //Assert
        Assert.Throws<ArgumentException>(a);
    }

    [Fact]
    public void DecrementQuantity_QuantityGreaterThenPossible_ArgumentOutOfRangeException()
    {
        //Arrange
        var startingQuantity = 2;
        var cartItem = new CartItem(Guid.NewGuid(), null, startingQuantity);

        //Act
        void a() => cartItem.DecrementQuantity(startingQuantity + 1);

        //Assert
        Assert.Throws<ArgumentOutOfRangeException>(a);
    }

    [Fact]
    public void EditNote_ValidPath_NoteEdited()
    {
        //Arrange
        var startingNote = "Initial Note";
        var expectedNote = "New Note";
        var cartItem = new CartItem(Guid.NewGuid(), startingNote);

        //Act
        cartItem.EditNote(expectedNote);

        //Assert
        Assert.Equal(expectedNote, cartItem.Note);
    }
}
