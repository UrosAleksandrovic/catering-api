using Catering.Domain.Aggregates.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Catering.Infrastructure.Data.EntityConfigurations;

internal class IdentityEntityConfiguration : IEntityTypeConfiguration<Identity>
{
    private const string TableName = "identities";

    public void Configure(EntityTypeBuilder<Identity> builder)
    {
        builder.ToTable(TableName);

        builder.HasKey(e => e.Id);

        builder.Property(e => e.Email)
            .IsRequired();

        var fullNameBuilder = builder.OwnsOne(e => e.FullName);
        fullNameBuilder.Property(e => e.FirstName).IsRequired();

        SeedData(builder);
    }

    private void SeedData(EntityTypeBuilder<Identity> builder)
    {
        builder.HasData(new
        {
            Id = "super.admin",
            Email = "super.admin@catering.test",
            Role = IdentityRole.SuperAdmin
        });
    }
}
