using System.ComponentModel.DataAnnotations;

namespace Catering.Infrastructure.Scheduling
{
    public class SchedulingDataSettings
    {
        public const string Position = "Persistence:Scheduling";

        [Required]
        public string ConnectionString { get; set; }
    }
}
