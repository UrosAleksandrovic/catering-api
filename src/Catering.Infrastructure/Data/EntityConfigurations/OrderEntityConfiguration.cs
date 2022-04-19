using Catering.Domain.Entities.OrderAggregate;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Catering.Infrastructure.Data.EntityConfigurations;

internal class OrderEntityConfiguration : IEntityTypeConfiguration<Order>
{
    public void Configure(EntityTypeBuilder<Order> builder)
    {
        builder.HasKey(e => e.Id);

        builder.Metadata
            .FindNavigation(nameof(Order.Items))
            .SetPropertyAccessMode(PropertyAccessMode.Field);
        builder.OwnsMany(e => e.Items);

        builder.OwnsOne(e => e.HomeDeliveryInfo);

        builder.Property(e => e.CustomerId).IsRequired();
    }
}
