using Catering.Application.Scheduling;
using Catering.Infrastructure.Scheduling.EntityConfigurations;
using Microsoft.EntityFrameworkCore;

namespace Catering.Infrastructure.Scheduling
{
    internal class SchedulingDbContext : DbContext
    {
        private const string SchemaName = "scheduling";

        public SchedulingDbContext(DbContextOptions<SchedulingDbContext> options)
            : base(options) { }

        internal DbSet<JobLog> JobLogs { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.HasDefaultSchema(SchemaName);

            new JobLogEntityConfiguration().Configure(modelBuilder.Entity<JobLog>());
        }
    }
}
