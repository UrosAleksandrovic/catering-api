namespace Catering.Infrastructure.Security.Options;

public class SecurityShaOptions
{
    public const string Position = "Security:Hashing:Key";

    public string Key { get; set; }
}
