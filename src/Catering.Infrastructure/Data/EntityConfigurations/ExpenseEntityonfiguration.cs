using Catering.Domain.Entities.ExpenseAggregate;
using Catering.Domain.Entities.IdentityAggregate;
using Catering.Domain.Entities.MenuAggregate;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Catering.Infrastructure.Data.EntityConfigurations;

internal class ExpenseEntityonfiguration : IEntityTypeConfiguration<Expense>
{
    public void Configure(EntityTypeBuilder<Expense> builder)
    {
        builder.HasKey(e => e.Id);

        builder.HasOne<Customer>()
               .WithOne()
               .HasForeignKey<Expense>(c => c.CustomerId);

        builder.Property(e => e.CustomerId).IsRequired();

        builder.HasOne<Menu>()
               .WithOne()
               .HasForeignKey<Expense>(c => c.MenuId);
    }
}
