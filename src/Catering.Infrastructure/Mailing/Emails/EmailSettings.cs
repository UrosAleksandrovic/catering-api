using System.ComponentModel.DataAnnotations;

namespace Catering.Infrastructure.Mailing.Emails;

public class EmailSettings
{
    public const string Position = "Mail:Email";

    [Required]
    public string Host { get; set; }

    [Range(0,ushort.MaxValue)]
    public int Port { get; set; }

    [Required]
    public string ServerUsername { get; set; }

    [Required]
    public string ServerPassword { get; set; }

    [Required]
    public string SystemSender { get; set; }

    public string SystemDisplayName { get; set; } = "Catering";
    public int RetryMaxLimit { get; set; } = 10;
}
