namespace Catering.Application.Mailing.Emails
{
    public interface IEmailSender
    {
        public Task SendAsync(Email email);
    }
}
