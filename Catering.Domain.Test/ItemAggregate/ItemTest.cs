using Catering.Domain.Entities.ItemAggregate;
using System;
using System.Linq;
using Xunit;

namespace Catering.Domain.Test.ItemAggregate;

public class ItemTest
{
    [Theory]
    [InlineData("")]
    [InlineData(" ")]
    public void EditGeneralData_NameIsInvalid_ArgumentException(string invalidName)
    {
        //Arrange
        var item = new Item("Name", "", 10, Guid.NewGuid());

        //Act
        void a() => item.EditGeneralData(invalidName, item.Description, item.Price);

        //Assert
        var exception = Assert.Throws<ArgumentException>(a);
        Assert.Equal("Required input name was empty. (Parameter 'name')", exception.Message);
    }

    [Fact]
    public void EditGeneralData_NameIsNull_ArgumentException()
    {
        //Arrange
        var expectedExceptionMessage = "Value cannot be null. (Parameter 'name')";
        var item = new Item("Name", "", 10, Guid.NewGuid());

        //Act
        void a() => item.EditGeneralData(null, item.Description, item.Price);

        //Assert
        var exception = Assert.Throws<ArgumentNullException>(a);
        Assert.Equal(expectedExceptionMessage, exception.Message);
    }

    [Theory]
    [InlineData(0)]
    [InlineData(-1)]
    public void EditGeneralData_PriceIsInvalid_ArgumentException(decimal invalidPrice)
    {
        //Arrange
        var expectedExceptionMessage = "Required input price cannot be zero or negative. (Parameter 'price')";
        var item = new Item("Name", "", 10, Guid.NewGuid());

        //Act
        void a() => item.EditGeneralData(item.Name, item.Description, invalidPrice);

        //Assert
        var exception = Assert.Throws<ArgumentException>(a);
        Assert.Equal(expectedExceptionMessage, exception.Message);
    }

    [Fact]
    public void EditGeneralData_ValidPath_ExpectedChanges()
    {
        //Arrange
        var expectedNewName = "New Name";
        var expectedNewDescription = "New Description";
        var expectedNewPrice = 10.2m;

        var item = new Item("Name", "", 10, Guid.NewGuid());

        //Act
        item.EditGeneralData(expectedNewName, expectedNewDescription, expectedNewPrice);

        //Assert
        Assert.Equal(expectedNewName, item.Name);
        Assert.Equal(expectedNewDescription, item.Description);
        Assert.Equal(expectedNewPrice, item.Price);
    }

    [Fact]
    public void AddCategories_NullEnumerable_ArgumentNullException()
    {
        //Arrange
        var item = new Item("Name", "", 10, Guid.NewGuid());

        //Act
        void a() => item.AddCategories(null);

        //Assert
        Assert.Throws<ArgumentNullException>(a);
    }

    [Fact]
    public void AddCategories_EmptyArrayPassed_CategoriesAreNotChanged()
    {
        //Arrange
        var item = new Item("Name", "", 10, Guid.NewGuid());

        //Act
        item.AddCategories(Enumerable.Empty<string>());

        //Assert
        Assert.Empty(item.Categories);
    }

    [Fact]
    public void AddCategories_ValidPath_CategoriesAdded()
    {
        //Arrange
        var categoriesToBeAdded = new[]
        {
                "pAsTa",
                "rissoto",
                "DRINK"
            };
        var item = new Item("Name", "", 10, Guid.NewGuid());

        //Act
        item.AddCategories(categoriesToBeAdded);
        item.AddCategories(new[] { "PASTA", "pasta", "Desert" });

        //Assert
        Assert.Equal(item.Categories.Count, categoriesToBeAdded.Length + 1);

        foreach (var category in categoriesToBeAdded)
            Assert.Contains(category.ToLowerInvariant(), item.Categories);
        Assert.Contains("desert", item.Categories);
    }

    [Fact]
    public void RemoveCategories_NullEnumerable_ArgumentNullException()
    {
        //Arrange
        var item = new Item("Name", "", 10, Guid.NewGuid());

        //Act
        void a() => item.RemoveCategories(null);

        //Assert
        Assert.Throws<ArgumentNullException>(a);
    }

    [Fact]
    public void RemoveCategories_EmptyArrayPassed_CategoriesAreNotChanged()
    {
        //Arrange
        var initialCategories = new[] { "something" };
        var item = new Item("Name", "", 10, Guid.NewGuid());
        item.AddCategories(initialCategories);

        //Act
        item.RemoveCategories(Enumerable.Empty<string>());

        //Assert
        Assert.Equal(1, item.Categories.Count);
        Assert.Equal(initialCategories, item.Categories);
    }

    [Fact]
    public void RemoveCategories_ValidPath_CategoriesRemoved()
    {
        //Arrange
        var initialCategories = new[] { "something", "new", "kinda" };
        var categoriesToRemove = new[] { "something", "SomeThing", "SOMETHING" };
        var item = new Item("Name", "", 10, Guid.NewGuid());
        item.AddCategories(initialCategories);

        //Act
        item.RemoveCategories(categoriesToRemove);

        //Assert
        Assert.Equal(2, item.Categories.Count);
        Assert.DoesNotContain("something", item.Categories);
    }

    [Fact]
    public void AddIngredients_NullEnumerable_ArgumentNullException()
    {
        //Arrange
        var item = new Item("Name", "", 10, Guid.NewGuid());

        //Act
        void a() => item.AddIngredients(null);

        //Assert
        Assert.Throws<ArgumentNullException>(a);
    }

    [Fact]
    public void AddIngredients_EmptyArrayPassed_IngredientsAreNotChanged()
    {
        //Arrange
        var item = new Item("Name", "", 10, Guid.NewGuid());

        //Act
        item.AddIngredients(Enumerable.Empty<string>());

        //Assert
        Assert.Empty(item.Categories);
    }

    [Fact]
    public void AddIngredients_ValidPath_IngredientsAdded()
    {
        //Arrange
        var ingredientsToBeAdded = new[]
        {
                "onion",
                "cheese",
                "garlic"
            };
        var item = new Item("Name", "", 10, Guid.NewGuid());

        //Act
        item.AddIngredients(ingredientsToBeAdded);
        item.AddIngredients(new[] { "Onion", "oNiOn", "carrot" });

        //Assert
        Assert.Equal(item.Ingredients.Count, ingredientsToBeAdded.Length + 1);

        foreach (var ingredient in ingredientsToBeAdded)
            Assert.Contains(ingredient.ToLowerInvariant(), item.Ingredients);
        Assert.Contains("carrot", item.Ingredients);
    }

    [Fact]
    public void RemoveIngredients_NullEnumerable_ArgumentNullException()
    {
        //Arrange
        var item = new Item("Name", "", 10, Guid.NewGuid());

        //Act
        void a() => item.RemoveIngredients(null);

        //Assert
        Assert.Throws<ArgumentNullException>(a);
    }

    [Fact]
    public void RemoveIngredients_EmptyArrayPassed_IngredientsAreNotChanged()
    {
        //Arrange
        var initialIngredients = new[] { "something" };
        var item = new Item("Name", "", 10, Guid.NewGuid());
        item.AddIngredients(initialIngredients);

        //Act
        item.RemoveIngredients(Enumerable.Empty<string>());

        //Assert
        Assert.Equal(1, item.Ingredients.Count);
        Assert.Equal(initialIngredients, item.Ingredients);
    }

    [Fact]
    public void RemoveIngredients_ValidPath_IngredientsRemoved()
    {
        //Arrange
        var initialIngredients = new[] { "something", "new", "kinda" };
        var ingredientsToRemove = new[] { "something", "SomeThing", "SOMETHING" };
        var item = new Item("Name", "", 10, Guid.NewGuid());
        item.AddIngredients(initialIngredients);

        //Act
        item.RemoveIngredients(ingredientsToRemove);

        //Assert
        Assert.Equal(2, item.Ingredients.Count);
        Assert.DoesNotContain("something", item.Ingredients);
    }

    [Fact]
    public void AddOrChangeRating_UserIdIsNull_ArgumentNullException()
    {
        //Arrange
        var item = new Item("Name", "", 10, Guid.NewGuid());

        //Act
        void a() => item.AddOrChangeRating(null, 2);

        //Assert
        Assert.Throws<ArgumentNullException>(a);
    }

    [Theory]
    [InlineData(ItemRating.MinimumRating - 1)]
    [InlineData(ItemRating.MaximumRating + 1)]
    public void AddOrChangeRating_RatingIsInvalid_ArgumentException(short invalidRating)
    {
        //Arrange
        var item = new Item("Name", "", 10, Guid.NewGuid());

        //Act
        void a() => item.AddOrChangeRating("id", invalidRating);

        //Assert
        Assert.Throws<ArgumentOutOfRangeException>(a);
    }

    [Fact]
    public void AddOrChangeRating_UserRatingDoesNotExist_RatingIsAdded()
    {
        //Arrange
        var userId = "id";
        var userRating = (short)5;
        var item = new Item("Name", "", 10, Guid.NewGuid());

        //Act
        item.AddOrChangeRating(userId, userRating);

        //Assert
        Assert.Equal(1, item.Ratings.Count);
        Assert.NotNull(item.Ratings.SingleOrDefault(s => s.UserId == userId && s.Rating == userRating));
    }

    [Fact]
    public void AddOrChangeRating_UserRatingExists_RatingIsChanged()
    {
        //Arrange
        var userId = "id";
        var newUserRating = (short)3;
        var item = new Item("Name", "", 10, Guid.NewGuid());

        //Act
        item.AddOrChangeRating(userId, 5);
        item.AddOrChangeRating(userId, newUserRating);

        //Assert
        Assert.Equal(1, item.Ratings.Count);
        Assert.NotNull(item.Ratings.SingleOrDefault(s => s.UserId == userId && s.Rating == newUserRating));
    }

    [Fact]
    public void RemoveRating_UserIdIsNull_NothingChanged()
    {
        //Arrange
        var item = new Item("Name", "", 10, Guid.NewGuid());
        item.AddOrChangeRating("id", 5);

        //Act
        item.RemoveRating(null);

        //Assert
        Assert.Equal(1, item.Ratings.Count);
    }

    [Fact]
    public void RemoveRating_UserIdDoesNotExist_NothingChanged()
    {
        //Arrange
        var item = new Item("Name", "", 10, Guid.NewGuid());
        item.AddOrChangeRating("id", 5);

        //Act
        item.RemoveRating("someId");

        //Assert
        Assert.Equal(1, item.Ratings.Count);
    }

    [Fact]
    public void RemoveRating_UserIdExists_RatingRemoved()
    {
        //Arrange
        var userId = "id";
        var item = new Item("Name", "", 10, Guid.NewGuid());
        item.AddOrChangeRating(userId, 5);

        //Act
        item.RemoveRating(userId);

        //Assert
        Assert.Empty(item.Ratings);
    }

    [Fact]
    public void TotalRating_NoRatings_AverageIsZero()
    {
        //Arrange
        var item = new Item("Name", "", 10, Guid.NewGuid());

        //Assert
        Assert.Equal(0, item.TotalRating);
    }

    [Fact]
    public void TotalRating_MultipleRatings_AverageRatingIsReturned()
    {
        //Arrange
        var item = new Item("Name", "", 10, Guid.NewGuid());
        item.AddOrChangeRating("1", 3);
        item.AddOrChangeRating("2", 4);
        item.AddOrChangeRating("3", 5);

        //Assert
        Assert.Equal(4, item.TotalRating);
    }
}
