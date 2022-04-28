using System.ComponentModel.DataAnnotations;

namespace Catering.Infrastructure.Security.Settings;

public class SecurityShaSettings
{
    public const string Position = "Security:Hashing:Key";

    [Required]
    public string Key { get; set; }
}
