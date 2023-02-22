using Catering.Domain.Entities.ItemAggregate;
using Catering.Domain.Entities.MenuAggregate;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Catering.Infrastructure.Data.EntityConfigurations;

internal class ItemEntityConfiguration : IEntityTypeConfiguration<Item>
{

    public void Configure(EntityTypeBuilder<Item> builder)
    {
        builder.HasKey(e => e.Id);

        builder.Property(e => e.Price)
            .HasColumnType("DECIMAL(19,4)");

        builder.Property(e => e.Name)
            .IsRequired();

        builder.OwnsMany(e => e.Ratings, cfg =>
            {
                cfg.HasKey(r => new { r.ItemId, r.CustomerId });
            });

        builder.Metadata
            .FindNavigation(nameof(Item.Ratings))
            .SetPropertyAccessMode(PropertyAccessMode.Field);

        builder.OwnsMany(e => e.Ingredients, cfg =>
        {
            cfg.HasKey(i => new { i.ItemId, i.Id });
        });

        builder.Metadata
            .FindNavigation(nameof(Item.Ingredients))
            .SetPropertyAccessMode(PropertyAccessMode.Field);

        builder.OwnsMany(e => e.Categories, cfg =>
        {
            cfg.HasKey(i => new { i.ItemId, i.Id });
        });

        builder.Metadata
            .FindNavigation(nameof(Item.Categories))
            .SetPropertyAccessMode(PropertyAccessMode.Field);


        builder.HasQueryFilter(e => !e.IsDeleted);

        builder.HasOne<Menu>()
            .WithMany()
            .HasForeignKey(e => e.MenuId);
    }
}
