namespace Catering.Infrastructure.Security.Settings;

internal class SecurityAesSettings
{
    public const string Position = "Security:Encryption:Aes";

    public string Key { get; set; }
    public string IV { get; set; }
}
