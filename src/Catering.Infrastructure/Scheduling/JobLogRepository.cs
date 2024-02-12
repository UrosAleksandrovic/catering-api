using Catering.Application.Scheduling;
using Microsoft.EntityFrameworkCore;

namespace Catering.Infrastructure.Scheduling
{
    internal class JobLogRepository : IJobLogRepository
    {
        private readonly IDbContextFactory<SchedulingDbContext> _dbContextFactory;

        public JobLogRepository(IDbContextFactory<SchedulingDbContext> dbContextFactory)
        {
            _dbContextFactory = dbContextFactory;
        }

        public async Task<JobLog> CreateAsync(string jobName)
        {
            using var dbContext = await _dbContextFactory.CreateDbContextAsync();

            var entityToSave = new JobLog
            {
                IsSuccessful = false,
                JobName = jobName
            };

            await dbContext.AddAsync(entityToSave);
            await dbContext.SaveChangesAsync();

            return entityToSave;
        }

        public async Task UpdateAsync(JobLog logToUpdate)
        {
            using var dbContext = await _dbContextFactory.CreateDbContextAsync();

            dbContext.Update(logToUpdate);
            await dbContext.SaveChangesAsync();
        }
    }
}
