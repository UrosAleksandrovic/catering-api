using Catering.Domain.Aggregates.Identity;
using Catering.Domain.Aggregates.Menu;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Catering.Infrastructure.Data.EntityConfigurations;

internal class MenuEntityConfiguration : IEntityTypeConfiguration<Menu>
{
    public void Configure(EntityTypeBuilder<Menu> builder)
    {
        builder.HasKey(e => e.Id);

        builder.Property(e => e.Name).IsRequired();

        var contactBuilder = builder.OwnsOne(e => e.Contact);
        contactBuilder.Property(c => c.Email).IsRequired();
        contactBuilder.HasOne<Identity>()
                    .WithMany()
                    .HasForeignKey(e => e.IdentityId);

        builder.HasQueryFilter(e => !e.IsDeleted);
    }
}
