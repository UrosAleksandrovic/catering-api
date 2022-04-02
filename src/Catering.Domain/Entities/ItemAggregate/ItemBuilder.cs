namespace Catering.Domain.Entities.ItemAggregate;

public class ItemBuilder : IBuilder<Item>
{
    private string _name;
    private string _description;
    private decimal _price;
    private Guid _menuId;
    private IEnumerable<string> _categories;
    private IEnumerable<string> _ingredients;
    private IEnumerable<ItemRating> _ratings;

    public Item Build()
    {
        var result = new Item(_name, _description, _price, _menuId);

        if (_categories != null && _categories.Any())
            result.AddCategories(_categories);

        if (_ingredients != null && _ingredients.Any())
            result.AddIngredients(_ingredients);

        if (_ratings != null && _ratings.Any())
            foreach (var rating in _ratings)
                result.AddRating(rating.UserId, rating.Rating);

        return result;
    }

    public ItemBuilder HasGeneralData(string name, string description, decimal price)
    {
        _name = name;
        _price = price;
        _description = description;

        return this;
    }

    public ItemBuilder HasCategories(IEnumerable<string> categories)
    {
        _categories = categories;

        return this;
    }

    public ItemBuilder HasRatings(IEnumerable<ItemRating> ratings)
    {
        _ratings = ratings;

        return this;
    }

    public ItemBuilder HasIngredients(IEnumerable<string> ingredients)
    {
        _ingredients = ingredients;

        return this;
    }

    public ItemBuilder HasMenu(Guid menuId)
    {
        _menuId = menuId;

        return this;
    }
}
