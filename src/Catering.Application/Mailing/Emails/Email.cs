namespace Catering.Application.Mailing.Emails;

public class Email : IMail
{
    public Guid Id { get; set; }
    public string Title { get; set; }
    public string TemplateContent { get; set; }
    public IEnumerable<EmailParameter> TemplateParameters { get; set; }
    public IEnumerable<string> Recepiants { get; set; }

    public string GetContent()
    {
        var content = TemplateContent;

        foreach (var parameter in TemplateParameters)
            content = content.Replace($"[[[{parameter.Name}]]]", parameter.Value);

        return content;
    }
}
