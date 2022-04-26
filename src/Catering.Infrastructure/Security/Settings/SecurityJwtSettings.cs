namespace Catering.Infrastructure.Security.Settings;

internal class SecurityJwtSettings
{
    public const string Position = "Security:Jwt";

    public string Key { get; set; }
    public string Issuer { get; set; }
    public int ExpirationInDays { get; set; }
}
