using System.ComponentModel.DataAnnotations;

namespace Catering.Infrastructure.Mailing;

public class MailingDataSettings
{
    public const string Position = "Persistence:Mailing";

    [Required]
    public string ConnectionString { get; set; }
}
