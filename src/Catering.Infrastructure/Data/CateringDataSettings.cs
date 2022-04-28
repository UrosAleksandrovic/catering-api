using System.ComponentModel.DataAnnotations;

namespace Catering.Infrastructure.Data;

public class CateringDataSettings
{
    public const string Position = "Persistance:Catering";

    [Required]
    public string ConnectionString { get; set; }
}
