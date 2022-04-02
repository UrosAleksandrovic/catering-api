using Ardalis.GuardClauses;

namespace Catering.Domain.Entities.ItemAggregate;

public class Item : BaseEntity
{
    public string Name { get; private set; }
    public string Description { get; private set; }
    public decimal Price { get; private set; }
    public Guid MenuId { get; private set; }

    private readonly List<string> _ingredients = new();
    public IReadOnlyList<string> Ingredients => _ingredients.AsReadOnly();

    private readonly List<ItemRating> _ratings = new();
    public IReadOnlyList<ItemRating> Ratings => _ratings.AsReadOnly();

    private readonly List<string> _categories = new();
    public IReadOnlyList<string> Categories => _categories.AsReadOnly();

    protected Item() { }

    public Item(string name, string description, decimal price, Guid menuId)
    {
        CheckForGeneralData(name, price);
        Guard.Against.Null(menuId);

        Id = Guid.NewGuid();
        Name = name;
        Description = description;
        Price = price;
        MenuId = menuId;
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

        var categoriesToAdd = newCategories.Except(_categories);
        _categories.AddRange(categoriesToAdd);
    }

    public void RemoveCategories(IEnumerable<string> categoriesToRemove)
    {
        Guard.Against.Null(categoriesToRemove);

        if (_categories.Count == 0)
            return;

        var resultCategories = _categories.Except(categoriesToRemove);
        _categories.Clear();
        _categories.AddRange(resultCategories);
    }

    public void AddIngredients(IEnumerable<string> newIngredients)
    {
        Guard.Against.Null(newIngredients);

        var ingredientsToAdd = newIngredients.Except(_ingredients);
        _ingredients.AddRange(ingredientsToAdd);
    }

    public void RemoveIngredients(IEnumerable<string> ingredientsToRemove)
    {
        Guard.Against.Null(ingredientsToRemove);

        if (_categories.Count == 0)
            return;

        var resultIngredients = _ingredients.Except(ingredientsToRemove);
        _ingredients.Clear();
        _ingredients.AddRange(resultIngredients);
    }

    public void AddRating(string userId, ushort rating)
    {
        _ratings.Add(new ItemRating(rating, userId));
    }

    public void RemoveRating(string userId)
    {
        var ratingToRemove = _ratings.SingleOrDefault(r => r.UserId == userId);

        if (ratingToRemove != null)
            _ratings.Remove(ratingToRemove);
    }

    public double TotalRating => Ratings.Average(r => r.Rating);

    private void CheckForGeneralData(string name, decimal price)
    {
        Guard.Against.NullOrWhiteSpace(name);
        Guard.Against.NegativeOrZero(price);
    }
}
