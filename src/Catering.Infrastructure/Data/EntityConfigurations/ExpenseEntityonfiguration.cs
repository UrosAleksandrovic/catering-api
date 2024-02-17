using Catering.Domain.Aggregates.Expense;
using Catering.Domain.Aggregates.Identity;
using Catering.Domain.Aggregates.Menu;
using Catering.Infrastructure.EFUtility;
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
