using Ardalis.GuardClauses;

namespace Catering.Domain.Entities.ItemAggregate;

public class ItemRating
{
    public const ushort MinimumRating = 0;
    public const ushort MaximumRating = 5;

    public ushort Rating { get; private set; }
    public string UserId { get; set; }

    public ItemRating(ushort rating, string userId)
    {
        Guard.Against.OutOfRange(rating, nameof(rating), MinimumRating, MaximumRating);
        Guard.Against.NullOrWhiteSpace(userId);

        Rating = rating;
        UserId = userId;
    }
}
