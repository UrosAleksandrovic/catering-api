using System.ComponentModel.DataAnnotations;

namespace Catering.Infrastructure.Security.Settings;

public class SecurityJwtSettings
{
    public const string Position = "Security:Jwt";

    [Required]
    public string Key { get; set; }

    public string Issuer { get; set; }
    public string Audience { get; set; }

    [Range(0, 30)]
    public int ExpirationInDays { get; set; }
}
