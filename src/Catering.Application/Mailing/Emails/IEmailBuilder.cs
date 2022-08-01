namespace Catering.Application.Mailing.Emails;

public interface IEmailBuilder : IMailBuilder<CateringEmail>
{
    IEmailBuilder HasEmailTemplate(EmailTemplate template, object root);
    IEmailBuilder HasRecepient(string recepientEmail);
    IEmailBuilder HasTitle(string title);
}