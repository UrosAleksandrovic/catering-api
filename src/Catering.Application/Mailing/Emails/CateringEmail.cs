namespace Catering.Application.Mailing.Emails;

public class CateringEmail : IMail
{
    public string Title { get; set; }
    public string TemplateName { get; set; }
    public IEnumerable<string> Recepiants { get; set; }
    public string Content { get; set; }
    public DateTimeOffset GeneratedOn { get; set; }
}
