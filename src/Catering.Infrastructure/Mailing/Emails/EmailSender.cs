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

    public async Task<bool> SendAsync(CateringEmail emailToSend)
    {
        var mailMessage = ConstructMailMessage(emailToSend);

        using var smtpClient = new SmtpClient();

        var numOfAttempts = 0;
        var isSuccessfullySent = false;
        while (numOfAttempts <= _options.RetryMaxLimit && !isSuccessfullySent)
        {
            try
            {
                await smtpClient.ConnectAsync(_options.Host, _options.Port, false);
                await smtpClient.AuthenticateAsync(_options.ServerUsername, _options.ServerPassword);

                await smtpClient.SendAsync(mailMessage);

                await smtpClient.DisconnectAsync(true);

                isSuccessfullySent = true;
            }
            catch (Exception e)
            {
                _logger.LogError(e, "Email message failed to be sent");
                numOfAttempts++;
            }
        }

        return isSuccessfullySent;
    }

    private MimeMessage ConstructMailMessage(CateringEmail emailToSend)
    {
        var contentBuilder = new BodyBuilder
        {
            HtmlBody = emailToSend.Content
        };

        var mailMessage = new MimeMessage();
        mailMessage.From.Add(new MailboxAddress(_options.SystemDisplayName, _options.SystemSender));
        mailMessage.To.AddRange(emailToSend.Recepiants.Select(e => new MailboxAddress(null, e)));
        mailMessage.Subject = emailToSend.Title;
        mailMessage.Body = contentBuilder.ToMessageBody();
        mailMessage.Date = DateTime.Now;

        return mailMessage;
    }
}
