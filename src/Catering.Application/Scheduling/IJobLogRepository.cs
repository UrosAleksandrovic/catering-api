namespace Catering.Application.Scheduling
{
    public interface IJobLogRepository
    {
        Task<JobLog> CreateAsync(string jobName);
        Task UpdateAsync(JobLog logToUpdate);
    }
}
