using Ardalis.GuardClauses;

namespace Catering.Domain.Entities.ItemAggregate;

public class ItemRating
{
    public const short MinimumRating = 0;
    public const short MaximumRating = 5;

    public short Rating { get; private set; }
    public string CustomerId { get; set; }

    private ItemRating() { }

    public ItemRating(short rating, string customerId)
    {
        CheckRatingValidity(rating);
        Guard.Against.NullOrWhiteSpace(customerId);

        Rating = rating;
        CustomerId = customerId;
    }

    public void EditRating(short newRating)
    {
        CheckRatingValidity(newRating);

        Rating = newRating;
    }

    private void CheckRatingValidity(short newRating)
    {
        Guard.Against.OutOfRange(newRating, nameof(newRating), MinimumRating, MaximumRating);
    }
}
