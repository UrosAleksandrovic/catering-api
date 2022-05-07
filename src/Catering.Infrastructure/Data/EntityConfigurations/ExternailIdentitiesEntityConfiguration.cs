using Catering.Domain.Entities.IdentityAggregate;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Catering.Infrastructure.Data.EntityConfigurations;

internal class ExternailIdentitiesEntityConfiguration : IEntityTypeConfiguration<ExternalIdentity>
{
    private const string TableName = "ExternalIdentities";

    public void Configure(EntityTypeBuilder<ExternalIdentity> builder)
    {
        builder.ToTable(TableName);

        builder
            .Property("_password")
            .HasColumnName("Password")
            .IsRequired();
    }
}
