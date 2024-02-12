namespace Catering.Application.Scheduling
{
    public interface IJob
    {
        Task<bool> ExecuteAsync();
        string GetName();
        int Priority { get; }
    }
}
