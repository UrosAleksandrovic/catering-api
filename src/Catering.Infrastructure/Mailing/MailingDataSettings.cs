using System.ComponentModel.DataAnnotations;

namespace Catering.Infrastructure.Mailing;

public class MailingDataSettings
{
    public const string Position = "Persistance:Mailing";

    [Required]
    public string ConnectionString { get; set; }
}
