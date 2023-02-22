using Ardalis.GuardClauses;

namespace Catering.Domain.Entities.ItemAggregate;

public class Item : ISoftDeletable
{
    public Guid Id { get; private set; }
    public string Name { get; private set; }
    public string Description { get; private set; }
    public decimal Price { get; private set; }
    public Guid MenuId { get; private set; }
    public bool IsDeleted { get; private set; }

    private readonly List<ItemIngredient> _ingredients = new();
    public IReadOnlyList<ItemIngredient> Ingredients => _ingredients.AsReadOnly();

    private readonly List<ItemRating> _ratings = new();
    public IReadOnlyList<ItemRating> Ratings => _ratings.AsReadOnly();

    private readonly List<ItemCategory> _categories = new();
    public IReadOnlyList<ItemCategory> Categories => _categories.AsReadOnly();

    protected Item() { }

    public Item(string name, string description, decimal price, Guid menuId)
    {
        CheckForGeneralData(name, price);
        Guard.Against.Default(menuId);

        Id = Guid.NewGuid();
        Name = name;
        Description = description;
        Price = price;
        MenuId = menuId;
        IsDeleted = false;
    }

    public void EditGeneralData(string name, string description, decimal price)
    {
        CheckForGeneralData(name, price);

        Name = name;
        Description = description;
        Price = price;
    }

    public void AddCategories(IEnumerable<string> newCategories)
    {
        Guard.Against.Null(newCategories);

        var newCategoriesWithoutDuplicates = newCategories.Select(c => c.ToLowerInvariant()).Distinct();
        var categoriesToAdd = newCategoriesWithoutDuplicates
            .Select(c => new ItemCategory(Id, c))
            .Except(Categories, new ItemCategoryComparator());

        _categories.AddRange(categoriesToAdd);
    }

    public void RemoveCategories(IEnumerable<string> categoriesToRemove)
    {
        Guard.Against.Null(categoriesToRemove);

        if (_categories.Count == 0)
            return;

        var categoriesWithoutDuplicates = categoriesToRemove.Select(c => c.ToLowerInvariant()).Distinct();
        var resultCategories = Categories
            .Except(categoriesWithoutDuplicates.Select(c => new ItemCategory(Id, c)), new ItemCategoryComparator())
            .ToArray();

        _categories.Clear();
        _categories.AddRange(resultCategories);
    }

    public void UpdateAllCategories(IEnumerable<string> listOfCategories)
    {
        var categoriesToRemove = Categories.Select(c => c.Id).Except(listOfCategories);
        RemoveCategories(categoriesToRemove);

        AddCategories(listOfCategories);
    }

    public void AddIngredients(IEnumerable<string> newIngredients)
    {
        Guard.Against.Null(newIngredients);

        var newIngredientsWithoutDuplicates = newIngredients.Select(c => c.ToLowerInvariant()).Distinct();
        var ingredientsToAdd = newIngredientsWithoutDuplicates
            .Select(c => new ItemIngredient(Id, c))
            .Except(Ingredients, new ItemIngredientComparator());

        _ingredients.AddRange(ingredientsToAdd);
    }

    public void RemoveIngredients(IEnumerable<string> ingredientsToRemove)
    {
        Guard.Against.Null(ingredientsToRemove);

        if (_ingredients.Count == 0)
            return;

        var ingredientsWithoutDuplicates = ingredientsToRemove.Select(c => c.ToLowerInvariant()).Distinct();
        var resultIngredients = Ingredients
            .Except(ingredientsWithoutDuplicates.Select(c => new ItemIngredient(Id, c)), new ItemIngredientComparator())
            .ToArray();

        _ingredients.Clear();
        _ingredients.AddRange(resultIngredients);
    }

    public void UpdateAllIngredients(IEnumerable<string> listOfIngredients)
    {
        var ingredientsToRemove = Ingredients.Select(c => c.Id).Except(listOfIngredients);
        RemoveIngredients(ingredientsToRemove);

        AddIngredients(listOfIngredients);
    }

    public void AddOrChangeRating(string customerId, short rating)
    {
        var customerRating = _ratings.SingleOrDefault(r => r.CustomerId == customerId);
        if (customerRating == null)
        {
            _ratings.Add(new ItemRating(rating, customerId));
            return;
        }

        customerRating.EditRating(rating);
    }

    public void RemoveRating(string customerId)
    {
        var ratingToRemove = _ratings.SingleOrDefault(r => r.CustomerId == customerId);

        if (ratingToRemove != null)
            _ratings.Remove(ratingToRemove);
    }

    public void MarkAsDeleted() => IsDeleted = true;

    public double TotalRating => Ratings.Any() ? Ratings.Average(r => r.Rating) : 0;

    private void CheckForGeneralData(string name, decimal price)
    {
        Guard.Against.NullOrWhiteSpace(name);
        Guard.Against.NegativeOrZero(price);
    }
}
