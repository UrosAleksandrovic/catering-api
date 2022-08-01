using Catering.Application.Mailing.Emails;
using Catering.Infrastructure.EFUtility;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Catering.Infrastructure.Mailing.Emails.EntityConfigurations;

internal class FailedEmailEntityConfiguration : IEntityTypeConfiguration<FailedCateringEmail>
{
    public void Configure(EntityTypeBuilder<FailedCateringEmail> builder)
    {
        builder.HasKey(e => e.Id);

        builder.Property(e => e.Title).IsRequired();

        builder.Property(e => e.Content).IsRequired();

        builder.Property(e => e.GeneratedOn)
            .HasConversion<DateTimeConverter>()
            .Metadata
            .SetValueComparer(typeof(DateTimeComparer));

        builder.Property(e => e.Recepiants)
            .HasConversion<StringEnumerableConverter>()
            .Metadata
            .SetValueComparer(typeof(StringEnumerableComparer));
    }
}
