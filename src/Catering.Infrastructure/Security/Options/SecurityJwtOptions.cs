namespace Catering.Infrastructure.Security.Options;

internal class SecurityJwtOptions
{
    public const string Position = "Security:Jwt";

    public string Key { get; set; }
    public string Issuer { get; set; }
    public int ExpirationInDays { get; set; }
}
