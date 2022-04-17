namespace Catering.Application.Mailing.Emails;

public class Email : IMail
{
    public string Content { get; set; }
    public IEnumerable<string> Recepiants { get; set; }
    public string Sender { get; set; }
}
