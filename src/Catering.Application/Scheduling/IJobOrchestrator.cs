namespace Catering.Application.Scheduling
{
    public interface IJobOrchestrator
    {
        Task RunAllJobsAsync();
    }
}
