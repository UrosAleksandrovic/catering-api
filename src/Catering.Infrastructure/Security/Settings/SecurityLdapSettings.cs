namespace Catering.Infrastructure.Security.Settings;

internal class SecurityLdapSettings
{
    public const string Position = "Security:LDAP";

    public string Server { get; set; }
    public string PortNumber { get; set; }
    public int DirectoryPath { get; set; }
}
