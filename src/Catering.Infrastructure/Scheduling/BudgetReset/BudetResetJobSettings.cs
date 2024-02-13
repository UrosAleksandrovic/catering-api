using System.ComponentModel.DataAnnotations;

namespace Catering.Infrastructure.Scheduling.BudgetReset;

internal class BudetResetJobSettings
{
    public const string Position = "Scheduling:BudgetReset";

    [Required]
    [Range(0, double.MaxValue)]
    public decimal DefaultValueEur { get; set; }

    [Required]
    public string CurrencyRateUrl { get; set; }

    [Required]
    public string TimeZoneOfExecutionIana { get; set; }

    [Required]
    public TimeSpan ExecutionStartTime { get; set; }

    [Required]
    public TimeSpan ExecutionEndTime { get; set; }
}
