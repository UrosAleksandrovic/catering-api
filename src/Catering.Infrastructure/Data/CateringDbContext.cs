using Catering.Domain.Aggregates.Cart;
using Catering.Domain.Aggregates.Expense;
using Catering.Domain.Aggregates.Identity;
using Catering.Domain.Aggregates.Item;
using Catering.Domain.Aggregates.Menu;
using Catering.Domain.Aggregates.Order;
using Catering.Infrastructure.Data.EntityConfigurations;
using Microsoft.EntityFrameworkCore;

namespace Catering.Infrastructure.Data;

internal class CateringDbContext : DbContext
{
    private const string SchemaName = "catering";

    public CateringDbContext(DbContextOptions<CateringDbContext> options) 
        : base(options) { }

    internal DbSet<Item> Items { get; set; }
    internal DbSet<Cart> Carts { get; set; }
    internal DbSet<Menu> Menus { get; set; }
    internal DbSet<Order> Orders { get; set; }
    internal DbSet<Identity> Identities { get; set; }
    internal DbSet<CateringIdentity> CateringIdentities { get; set; }
    internal DbSet<IdentityInvitation> IdentityInvitations { get; set; }
    internal DbSet<Customer> Customers { get; set; }
    internal DbSet<Expense> Expenses { get; set; } 

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.HasDefaultSchema(SchemaName);

        modelBuilder.ApplyConfiguration(new ItemEntityConfiguration());
        modelBuilder.ApplyConfiguration(new MenuEntityConfiguration());
        modelBuilder.ApplyConfiguration(new CartEntityConfiguration());
        modelBuilder.ApplyConfiguration(new OrderEntityConfiguration());
        modelBuilder.ApplyConfiguration(new IdentityEntityConfiguration());
        modelBuilder.ApplyConfiguration(new CateringIdentitiesConfiguration());
        modelBuilder.ApplyConfiguration(new CustomerEntityConfiguration());
        modelBuilder.ApplyConfiguration(new IdentityInvitationEntityConfiguration());
    }
}
