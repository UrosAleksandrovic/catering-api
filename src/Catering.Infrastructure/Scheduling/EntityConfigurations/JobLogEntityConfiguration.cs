using Catering.Application.Scheduling;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Catering.Infrastructure.Scheduling.EntityConfigurations;

internal class JobLogEntityConfiguration : IEntityTypeConfiguration<JobLog>
{
    public void Configure(EntityTypeBuilder<JobLog> builder)
    {
        builder.HasKey(builder => builder.Id);

        builder.Property(builder => builder.Id)
            .ValueGeneratedOnAdd();

        builder.Property(builder => builder.GeneratedAt)
            .HasDefaultValueSql("CURRENT_TIMESTAMP");
    }
}
