using Catering.Domain.Entities.OrderAggregate;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Catering.Infrastructure.Data.EntityConfigurations;

internal class OrderEntityConfiguration : IEntityTypeConfiguration<Order>
{
    public void Configure(EntityTypeBuilder<Order> builder)
    {
        builder.HasKey(e => e.Id);

        builder.OwnsMany(e => e.Items);

        builder.Metadata
            .FindNavigation(nameof(Order.Items))
            .SetPropertyAccessMode(PropertyAccessMode.Field);

        var homeDeliveryInfoBuilder = builder.OwnsOne(e => e.HomeDeliveryInfo);
        homeDeliveryInfoBuilder.Property(e => e.StreetAndHouse).IsRequired();

        builder.Property(e => e.CustomerId).IsRequired();
    }
}
