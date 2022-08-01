using Catering.Application.Mailing.Emails;
using Catering.Infrastructure.Mailing.Emails.EntityConfigurations;
using Microsoft.EntityFrameworkCore;

namespace Catering.Infrastructure.Mailing;

internal class MailingDbContext : DbContext
{
    private const string SchemaName = "mailing";

    public MailingDbContext(DbContextOptions options) 
        : base(options) { }

    internal DbSet<EmailTemplate> Templates { get; set; }
    internal DbSet<FailedCateringEmail> FailedEmails { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.HasDefaultSchema(SchemaName);

        new FailedEmailEntityConfiguration().Configure(modelBuilder.Entity<FailedCateringEmail>());
        new EmailTemplateEntityConfiguration().Configure(modelBuilder.Entity<EmailTemplate>());
    }
}
