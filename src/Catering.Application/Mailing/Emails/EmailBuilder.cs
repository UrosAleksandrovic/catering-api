using Ardalis.GuardClauses;
using Catering.Domain.Builders;

namespace Catering.Application.Mailing.Emails;

public class EmailBuilder : IBuilder<Email>
{
    private EmailTemplate _template;
    private readonly Dictionary<string, string> _parameters = new();
    private readonly List<string> _recepients = new();

    //TODO: Should I take this from configuration
    private const string _systemSender = "";


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

        _parameters.Add(parameterName, value);

        return this;
    }

    public EmailBuilder HasRecepient(string recepientEmail)
    {
        Guard.Against.NullOrWhiteSpace(recepientEmail);

        _recepients.Add(recepientEmail);

        return this;
    }

    public Email Build()
    {
        var content = _template.HtmlTemplate;

        foreach(var parameter in _parameters)
            content = content.Replace($"[[[{parameter.Key}]]]", parameter.Value);

        return new Email
        {
            Content = content,
            Recepiants = _recepients,
            Sender = _systemSender
        };
    }

    public void Reset()
    {
        _recepients.Clear();
        _parameters.Clear();
        _template = null;
    }
}
