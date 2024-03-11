using Catering.Domain.Aggregates.Item;
using System;
using Xunit;

namespace Catering.Domain.Test.ItemAggregate;

public class ItemRatingTest
{
    [Theory]
    [InlineData(ItemRating.MinimumRating - 1)]
    [InlineData(ItemRating.MaximumRating + 1)]
    public void EditRating_RatingInvalid_ArgumentOurOfRangeException(short invalidRating)
    {
        //Arrange
        var rating = new ItemRating(5, "customerId");

        //Act
        void a() => rating.EditRating(invalidRating);

        Assert.Throws<ArgumentOutOfRangeException>(a);
    }

    [Fact]
    public void EditRating_ValidPath_RatingChanged()
    {
        //Arrange
        var newRating = (short)3;
        var rating = new ItemRating(5, "customerId");

        //Act
        rating.EditRating(newRating);

        Assert.Equal(newRating, rating.Rating);
    }
}
