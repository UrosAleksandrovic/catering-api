using Catering.Domain.Entities.CartAggregate;
using Catering.Domain.Entities.IdentityAggregate;
using Catering.Domain.Entities.ItemAggregate;
using Catering.Domain.Entities.MenuAggregate;
using Catering.Domain.Entities.OrderAggregate;
using Catering.Infrastructure.Data.EntityConfigurations;
using Microsoft.EntityFrameworkCore;

namespace Catering.Infrastructure.Data;

internal class CateringDbContext : DbContext
{
    private const string SchemaName = "catering";

    public CateringDbContext(DbContextOptions options) 
        : base(options) { }

    internal DbSet<Item> Items { get; set; }
    internal DbSet<Cart> Carts { get; set; }
    internal DbSet<Menu> Menus { get; set; }
    internal DbSet<Order> Orders { get; set; }
    internal DbSet<Identity> Identities { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.HasDefaultSchema(SchemaName);

        new ItemEntityConfiguration().Configure(modelBuilder.Entity<Item>());
        new MenuEntityConfiguration().Configure(modelBuilder.Entity<Menu>());
        new CartEntityConfiguration().Configure(modelBuilder.Entity<Cart>());
        new OrderEntityConfiguration().Configure(modelBuilder.Entity<Order>());
        new IdentityEntityConfiguration().Configure(modelBuilder.Entity<Identity>());
        new ExternailIdentitiesEntityConfiguration().Configure(modelBuilder.Entity<ExternalIdentity>());
    }
}
