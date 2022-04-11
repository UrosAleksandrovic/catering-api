using Ardalis.GuardClauses;

namespace Catering.Domain.Entities.ItemAggregate;

public class ItemRating
{
    public const short MinimumRating = 0;
    public const short MaximumRating = 5;

    public short Rating { get; private set; }
    public string UserId { get; set; }

    private ItemRating() { }

    public ItemRating(short rating, string userId)
    {
        CheckRatingValidity(rating);
        Guard.Against.NullOrWhiteSpace(userId);

        Rating = rating;
        UserId = userId;
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
