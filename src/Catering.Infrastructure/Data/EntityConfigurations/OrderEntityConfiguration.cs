using Catering.Domain.Aggregates.Order;
using Catering.Infrastructure.EFUtility;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Catering.Infrastructure.Data.EntityConfigurations;

internal class OrderEntityConfiguration : IEntityTypeConfiguration<Order>
{
    public void Configure(EntityTypeBuilder<Order> builder)
    {
        builder.HasKey(e => e.Id);

        builder.OwnsMany(e => e.Items, cfg =>
        {
            cfg.HasKey(i => new { i.OrderId, i.ItemId });
            cfg.Property(p => p.NameSnapshot).IsRequired();
        });

        builder.Metadata
            .FindNavigation(nameof(Order.Items))
            .SetPropertyAccessMode(PropertyAccessMode.Field);

        var homeDeliveryInfoBuilder = builder.OwnsOne(e => e.HomeDeliveryInfo);
        homeDeliveryInfoBuilder.Property(e => e.StreetAndHouse).IsRequired();

        builder.Property(e => e.CustomerId).IsRequired();

        builder.Property(e => e.ExpectedOn)
            .HasConversion<DateTimeConverter>()
            .Metadata
            .SetValueComparer(typeof(DateTimeComparer));

        builder.Property(e => e.CreatedOn)
            .HasConversion<DateTimeConverter>()
            .Metadata
            .SetValueComparer(typeof(DateTimeComparer));
    }
}
