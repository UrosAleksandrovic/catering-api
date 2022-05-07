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

        builder
            .Property(e => e.Price)
            .HasColumnType("DECIMAL(19,4)");

        builder.Property(e => e.Name)
            .IsRequired();

        builder.Metadata.FindNavigation(nameof(Item.Ratings))
            .SetPropertyAccessMode(PropertyAccessMode.Field);

        builder
            .OwnsMany(e => e.Ratings)
            .HasIndex(r => r.CustomerId);

        builder.Metadata
            .FindNavigation(nameof(Item.Ingredients))
            .SetPropertyAccessMode(PropertyAccessMode.Field);

        builder
            .Property(e => e.Ingredients)
            .HasColumnType("nvarchar")
            .HasConversion<StringEnumerationConverter>();

        builder.Metadata
            .FindNavigation(nameof(Item.Categories))
            .SetPropertyAccessMode(PropertyAccessMode.Field);

        //TODO: Might be a bad idea, think about this one
        builder
            .Property(e => e.Categories)
            .HasColumnType("nvarchar")
            .HasConversion<StringEnumerationConverter>();

        builder.HasQueryFilter(e => !e.IsDeleted);

        builder.HasOne<Menu>()
            .WithMany()
            .HasForeignKey(e => e.MenuId);
    }
}
