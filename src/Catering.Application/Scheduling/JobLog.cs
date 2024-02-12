namespace Catering.Application.Scheduling
{
    public class JobLog
    {
        public Guid Id { get; set; }
        public DateTimeOffset GeneratedAt { get; set; }
        public DateTimeOffset? ExecutedAt { get; set; }
        public DateTimeOffset? TargetedAt { get; set; }
        public string JobName { get; set; }
        public bool IsSuccessful { get; set; }
    }
}
