namespace Catering.Application.Mailing.Emails;

public interface IEmailRepository
{
    Task<EmailTemplate> GetTemplateAsync(string templateName);
    Task<Email> SaveAsFailedEmailAsync(Email message);
}
