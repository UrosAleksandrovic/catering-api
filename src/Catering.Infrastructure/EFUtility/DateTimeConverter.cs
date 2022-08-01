using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace Catering.Infrastructure.EFUtility;

internal class DateTimeConverter : ValueConverter<DateTime, DateTime>
{
    public DateTimeConverter()
        : base(v => v.ToUniversalTime(),
              v => DateTime.SpecifyKind(v, DateTimeKind.Utc))
    { }
}

internal class DateTimeComparer : ValueComparer<DateTime>
{
    public DateTimeComparer()
        : base((v1, v2) => v1.Equals(v2),
            v => v.GetHashCode(),
            v => new DateTime(v.Ticks))
    { }
}

internal class NullableDateTimeConverter : ValueConverter<DateTime?, DateTime?>
{
    public NullableDateTimeConverter()
        : base(v => v.HasValue ? v.Value.ToUniversalTime() : v,
               v => v.HasValue ? DateTime.SpecifyKind(v.Value, DateTimeKind.Utc) : v)
    { }
}

internal class NullableDateTimeComparer : ValueComparer<DateTime>
{
    public NullableDateTimeComparer()
        : base((v1, v2) => v1.Equals(v2),
            v => v.GetHashCode(),
            v => new DateTime(v.Ticks))
    { }
}
