using Catering.Application.Mailing.Emails;
using Catering.Infrastructure.Mailing.Emails;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Catering.DependencyInjection;

public static class MailingExtensions
{
    public static IServiceCollection AddEmailSending(
        this IServiceCollection services,
        IConfiguration configuration)
    {
        var settingsSection = configuration.GetSection(EmailSettings.Position);

        services.AddOptions<EmailSettings>()
            .Bind(settingsSection)
            .ValidateDataAnnotations()
            .ValidateOnStart();

        services.AddTransient<IEmailSender, EmailSender>();
        services.AddTransient<IEmailBuilder, EmailBuilder>();

        return services;
    }
}
