using System.ComponentModel.DataAnnotations;

namespace Catering.Infrastructure.Security.Settings;

internal class SecurityAesSettings
{
    public const string Position = "Security:Encryption:Aes";

    [Required]
    public string Key { get; set; }

    [Required]
    public string IV { get; set; }
}
