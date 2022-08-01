using Ardalis.GuardClauses;
using Catering.Application.Mailing.Emails;
using EazyTemplate;

namespace Catering.Infrastructure.Mailing.Emails;

internal class EmailBuilder : IEmailBuilder
{
    private string _title;
    private string _content;
    private string _templateName;
    private readonly List<string> _recepients = new();

    public IEmailBuilder HasEmailTemplate(EmailTemplate template, object root)
    {
        Guard.Against.Default(template);

        var templateBuilder = new TextBuilder();
        templateBuilder.HasTemplate(template.HtmlTemplate);
        _content = templateBuilder.BuildBody(root);

        _templateName = template.Name;

        return this;
    }

    public IEmailBuilder HasRecepient(string recepientEmail)
    {
        Guard.Against.NullOrWhiteSpace(recepientEmail);

        if (!_recepients.Contains(recepientEmail))
            _recepients.Add(recepientEmail);

        return this;
    }

    public IEmailBuilder HasTitle(string title)
    {
        Guard.Against.NullOrWhiteSpace(title);

        _title = title;

        return this;
    }

    public CateringEmail Build()
    {
        return new CateringEmail
        {
            GeneratedOn = DateTime.Now,
            Recepiants = _recepients,
            Title = _title,
            TemplateName = _templateName,
            Content = _content
        };
    }

    public void Reset()
    {
        _recepients.Clear();
        _templateName = null;
        _title = null;
        _content = null;
    }
}
