using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace Catering.Infrastructure.Data.EntityConfigurations;

internal class StringListConverter : ValueConverter<IReadOnlyList<string>, string>
{
    public const char EnumerationSeparator = '|';
    public StringListConverter() 
        : base(
            v => string.Join(EnumerationSeparator, v),
            v => v.Split(EnumerationSeparator, StringSplitOptions.TrimEntries).ToList())
    {
    }
}

internal class StringListComparer : ValueComparer<IReadOnlyList<string>>
{
    public StringListComparer() 
        : base((v1, v2) => v1.SequenceEqual(v2),
                v => v.Aggregate(0, (a, v) => HashCode.Combine(a, v.GetHashCode())),
                v => v.ToList())
    {
    }
}
