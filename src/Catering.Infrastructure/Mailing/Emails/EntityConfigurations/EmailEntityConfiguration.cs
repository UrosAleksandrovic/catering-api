using Catering.Application.Mailing.Emails;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Catering.Infrastructure.Mailing.Emails.EntityConfigurations;

internal class EmailEntityConfiguration : IEntityTypeConfiguration<Email>
{
    public void Configure(EntityTypeBuilder<Email> builder)
    {
        builder.HasKey(e => e.Id);

        builder.Property(e => e.Title).IsRequired();
        builder.Property(e => e.TemplateContent).IsRequired();

        builder.OwnsMany(e => e.Recepiants);
        builder.HasMany(e => e.TemplateParameters).WithOne();
    }
}
