﻿using Catering.Domain.Entities.CartAggregate;
using Catering.Domain.Entities.IdentityAggregate;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Catering.Infrastructure.Data.EntityConfigurations;

internal class CartEntityConfiguration : IEntityTypeConfiguration<Cart>
{
    public void Configure(EntityTypeBuilder<Cart> builder)
    {
        builder.HasKey(e => e.Id);

        builder
            .HasOne<Customer>()
            .WithOne()
            .HasForeignKey<Cart>(c => c.CustomerId);

        builder.Property(e => e.CustomerId).IsRequired();

        builder.Metadata
            .FindNavigation(nameof(Cart.Items))
            .SetPropertyAccessMode(PropertyAccessMode.Field);
        builder.OwnsMany(e => e.Items);
    }
}
