using Catering.Application.Mailing.Emails;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Catering.Infrastructure.Mailing.Emails.EntityConfigurations;

internal class EmailParameterEntityConfiguration : IEntityTypeConfiguration<EmailParameter>
{
    public void Configure(EntityTypeBuilder<EmailParameter> builder)
    {
        builder.HasKey(e => new { e.EmailId, e.Name });

        builder.Property(e => e.Name).IsRequired();
    }
}
