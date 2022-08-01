using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace Catering.Infrastructure.EFUtility;

internal class StringReadonlyListConverter : ValueConverter<IReadOnlyList<string>, string>
{
    public const char EnumerationSeparator = '|';
    public StringReadonlyListConverter()
        : base(
            v => string.Join(EnumerationSeparator, v),
            v => v.Split(EnumerationSeparator, StringSplitOptions.TrimEntries).ToList())
    { }
}

internal class StringReadonlyListComparer : ValueComparer<IReadOnlyList<string>>
{
    public StringReadonlyListComparer()
        : base((v1, v2) => v1.SequenceEqual(v2),
                v => v.Aggregate(0, (a, v) => HashCode.Combine(a, v.GetHashCode())),
                v => v.ToList())
    { }
}

internal class StringEnumerableConverter : ValueConverter<IEnumerable<string>, string>
{
    public const char EnumerationSeparator = '|';
    public StringEnumerableConverter()
        : base(
            v => string.Join(EnumerationSeparator, v),
            v => v.Split(EnumerationSeparator, StringSplitOptions.TrimEntries).ToList())
    { }
}

internal class StringEnumerableComparer : ValueComparer<IEnumerable<string>>
{
    public StringEnumerableComparer()
        : base((v1, v2) => v1.SequenceEqual(v2),
                v => v.Aggregate(0, (a, v) => HashCode.Combine(a, v.GetHashCode())),
                v => v.ToList())
    { }
}
