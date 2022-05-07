namespace Catering.Application.Mailing.Emails;

public class Email : IMail
{
    public string Title { get; set; }
    public string Content { get; set; }
    public IEnumerable<string> Recepiants { get; set; }
}
