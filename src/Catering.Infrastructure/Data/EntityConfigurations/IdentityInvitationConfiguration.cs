﻿using Catering.Domain.Entities.IdentityAggregate;
using Catering.Infrastructure.EFUtility;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Catering.Infrastructure.Data.EntityConfigurations;

internal class IdentityInvitationEntityConfiguration : IEntityTypeConfiguration<IdentityInvitation>
{
    public void Configure(EntityTypeBuilder<IdentityInvitation> builder)
    {
        builder.HasKey(e => e.Id);

        builder.Property(e => e.Email).IsRequired();

        var fullNameBuilder = builder.OwnsOne(e => e.FullName);
        fullNameBuilder.Property(e => e.FirstName).IsRequired();

        builder.Property(e => e.CreatedOn)
            .HasConversion<DateTimeConverter>()
            .Metadata
            .SetValueComparer(typeof(DateTimeComparer));

        builder.Property(e => e.ExpiredOn)
            .HasConversion<DateTimeConverter>()
            .Metadata
            .SetValueComparer(typeof(DateTimeComparer));
    }
}