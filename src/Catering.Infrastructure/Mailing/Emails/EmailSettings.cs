namespace Catering.Infrastructure.Mailing.Emails;

public class EmailSettings
{
    public const string Position = "Mail:Email";

    public string Host { get; set; }
    public int Port { get; set; }
    public string ServerUsername { get; set; }
    public string ServerPassword { get; set; }

    public string SystemSender { get; set; }
    public string SystemDisplayName { get; set; }
    public int RetryMaxLimit { get; set; } = 10;
}
