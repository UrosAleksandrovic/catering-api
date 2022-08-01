using Catering.Domain.Entities.ItemAggregate;
using Catering.Domain.Entities.MenuAggregate;
using Catering.Infrastructure.EFUtility;
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

        builder.Property(e => e.Ingredients)
            .Metadata
            .SetPropertyAccessMode(PropertyAccessMode.Field);

        builder.Property(e => e.Ingredients)
            .HasColumnType("text")
            .HasConversion<StringReadonlyListConverter>()
            .Metadata
            .SetValueComparer(typeof(StringReadonlyListComparer));

        builder.Property(e => e.Categories)
            .Metadata
            .SetPropertyAccessMode(PropertyAccessMode.Field);

        builder
            .Property(e => e.Categories)
            .HasColumnType("text")
            .HasConversion<StringReadonlyListConverter>()
            .Metadata
            .SetValueComparer(typeof(StringReadonlyListComparer));

        builder.HasQueryFilter(e => !e.IsDeleted);

        builder.HasOne<Menu>()
            .WithMany()
            .HasForeignKey(e => e.MenuId);
    }
}
