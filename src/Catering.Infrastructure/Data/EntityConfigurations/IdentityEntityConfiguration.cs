using Catering.Domain.Entities.IdentityAggregate;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Catering.Infrastructure.Data.EntityConfigurations;

internal class IdentityEntityConfiguration : IEntityTypeConfiguration<Identity>
{
    private const string TableName = "Identities";

    public void Configure(EntityTypeBuilder<Identity> builder)
    {
        builder.ToTable(TableName);

        builder.HasKey(e => e.Id);

        builder.Property(e => e.Email)
            .IsRequired();

        builder.OwnsOne(e => e.FullName);
    }
}
