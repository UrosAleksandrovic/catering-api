using System.ComponentModel.DataAnnotations;

namespace Catering.Infrastructure.Security.Settings;

internal class SecurityLdapSettings
{
    public const string Position = "Security:LDAP";

    [Required]
    public string Server { get; set; }

    [Required]
    public string PortNumber { get; set; }

    [Required]
    public string DirectoryPath { get; set; }
}
