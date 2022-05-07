using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace Catering.Infrastructure.Data.EntityConfigurations;

internal class StringEnumerationConverter : ValueConverter<IEnumerable<string>, string>
{
    public const char EnumerationSeparator = '|';
    public StringEnumerationConverter() 
        : base(
            v => string.Join(EnumerationSeparator, v),
            v => v.Split(EnumerationSeparator, StringSplitOptions.TrimEntries))
    {
    }
}
