using Catering.Domain.Entities.IdentityAggregate;
using Catering.Infrastructure.EFUtility;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
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

        var fullNameBuilder = builder.OwnsOne(e => e.FullName);
        fullNameBuilder.Property(e => e.FirstName).IsRequired();
    }
}
