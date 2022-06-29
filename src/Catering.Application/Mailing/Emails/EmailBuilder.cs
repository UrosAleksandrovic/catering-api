using Ardalis.GuardClauses;
using Catering.Domain.Builders;

namespace Catering.Application.Mailing.Emails;

public class EmailBuilder : IBuilder<Email>
{
    private EmailTemplate _template;
    private string _title;
    private readonly Dictionary<string, string> _parameters = new();
    private readonly List<string> _recepients = new();

    public EmailBuilder HasEmailTemplate(EmailTemplate template)
    {
        Guard.Against.Default(template);

        _template = template;

        return this;
    }

    public EmailBuilder HasParameter(string parameterName, string value)
    {
        Guard.Against.NullOrWhiteSpace(parameterName);
        Guard.Against.NullOrWhiteSpace(value);

        var parameterExists = _parameters.TryGetValue(parameterName, out var oldValue);
        if (parameterExists)
            _parameters[parameterName] = value;
        else
            _parameters.Add(parameterName, value);

        return this;
    }

    public EmailBuilder HasRecepient(string recepientEmail)
    {
        Guard.Against.NullOrWhiteSpace(recepientEmail);

        _recepients.Add(recepientEmail);

        return this;
    }

    public EmailBuilder HasTitle(string title)
    {
        Guard.Against.NullOrWhiteSpace(title);

        _title = title;

        return this;
    }

    public Email Build()
    {
        return new Email
        {
            TemplateContent = _template.HtmlTemplate,
            TemplateParameters = _parameters.Select(kv => new EmailParameter { Name = kv.Key, Value = kv.Value}),
            Recepiants = _recepients,
            Title = _title
        };
    }

    public void Reset()
    {
        _recepients.Clear();
        _parameters.Clear();
        _template = null;
        _title = null;
    }
}
