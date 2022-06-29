using Catering.Application.Mailing.Emails;
using MailKit.Net.Smtp;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using MimeKit;

namespace Catering.Infrastructure.Mailing.Emails;

internal class EmailSender : IEmailSender
{
    private readonly EmailSettings _options;
    private readonly ILogger<EmailSender> _logger;

    public EmailSender(IOptions<EmailSettings> options, ILogger<EmailSender> logger)
    {
        _options = options.Value;
        _logger = logger;
    }

    public async Task<bool> SendAsync(Email message)
    {
        var mailMessage = ConstructMailMessage(message);

        using var smtpClient = new SmtpClient();

        var numOfAttempts = 0;
        while (numOfAttempts <= _options.RetryMaxLimit)
        {
            try
            {
                await smtpClient.ConnectAsync(_options.Host, _options.Port, false);
                await smtpClient.AuthenticateAsync(_options.ServerUsername, _options.ServerPassword);

                await smtpClient.SendAsync(mailMessage);

                await smtpClient.DisconnectAsync(true);
            }
            catch (Exception e)
            {
                _logger.LogError(e, "Email message failed to be sent");
            }
            finally
            {
                numOfAttempts++;
            }
        }

        return numOfAttempts < _options.RetryMaxLimit;
    }

    private MimeMessage ConstructMailMessage(Email cateringEmail)
    {
        var contentBuilder = new BodyBuilder();
        contentBuilder.HtmlBody = cateringEmail.GetContent();

        var mailMessage = new MimeMessage();
        mailMessage.From.Add(new MailboxAddress(_options.SystemDisplayName, _options.SystemSender));
        mailMessage.To.AddRange(cateringEmail.Recepiants.Select(e => new MailboxAddress(null, e)));
        mailMessage.Subject = cateringEmail.Title;
        mailMessage.Body = contentBuilder.ToMessageBody();
        mailMessage.Date = DateTime.Now;

        return mailMessage;
    }
}
