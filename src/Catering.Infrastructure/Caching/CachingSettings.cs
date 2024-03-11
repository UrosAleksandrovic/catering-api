using System.ComponentModel.DataAnnotations;

namespace Catering.Infrastructure.Caching;

internal class CachingSettings
{
    public const string Position = "Caching";
    public string ConnectionString { get; set; }

    [Required]
    public bool IsInMemory { get; set; }

    [Required]
    public int ExpirationInSeconds { get; set; }
}
