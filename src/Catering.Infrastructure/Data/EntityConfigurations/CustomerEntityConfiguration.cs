using Catering.Domain.Entities.IdentityAggregate;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Catering.Infrastructure.Data.EntityConfigurations;

internal class CustomerEntityConfiguration : IEntityTypeConfiguration<Customer>
{
    private const string TableName = "Customers";

    public void Configure(EntityTypeBuilder<Customer> builder)
    {
        builder.ToTable(TableName);

        var budgetBuidler = builder.OwnsOne(e => e.Budget);

        budgetBuidler.Property(e => e.ReservedAssets)
            .HasColumnType("decimal(19,4)");

        budgetBuidler.Property(e => e.Balance)
            .HasColumnType("decimal(19,4)");
    }
}
