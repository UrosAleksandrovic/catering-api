using Catering.Domain.Entities.IdentityAggregate;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Catering.Infrastructure.Data.EntityConfigurations;

internal class CateringIdentitiesConfiguration : IEntityTypeConfiguration<CateringIdentity>
{
    private const string TableName = "catering_identities";

    public void Configure(EntityTypeBuilder<CateringIdentity> builder)
    {
        builder.ToTable(TableName);

        builder
            .Property("_password")
            .HasColumnName("Password")
            .IsRequired();
    }
}
