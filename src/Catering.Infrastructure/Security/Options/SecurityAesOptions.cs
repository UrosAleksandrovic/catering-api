namespace Catering.Infrastructure.Security.Options;

internal class SecurityAesOptions
{
    public const string Position = "Security:Encryption:Aes";

    public string Key { get; set; }
    public string IV { get; set; }
}
