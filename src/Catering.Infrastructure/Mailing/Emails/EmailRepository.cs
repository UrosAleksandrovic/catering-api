using Catering.Application.Mailing.Emails;
using Catering.Application.Security;
using Microsoft.EntityFrameworkCore;

namespace Catering.Infrastructure.Mailing.Emails;

internal class EmailRepository : IEmailRepository
{
    private readonly IDbContextFactory<MailingDbContext> _dbContextFactory;
    private readonly IDataProtector _dataProtector;

    public EmailRepository(
        IDbContextFactory<MailingDbContext> dbContextFactory,
        IDataProtector dataProtector)
    {
        _dbContextFactory = dbContextFactory;
        _dataProtector = dataProtector;
    }

    public async Task<EmailTemplate> GetTemplateAsync(string templateName)
    {
        using var dbContext = await _dbContextFactory.CreateDbContextAsync();

        return await dbContext.Templates.AsNoTracking().FirstOrDefaultAsync(t => t.Name == templateName);
    }

    public async Task<Email> SaveAsFailedEmailAsync(Email message)
    {
        using var dbContext = await _dbContextFactory.CreateDbContextAsync();

        var entityToSave = new Email
        {
            Content = _dataProtector.Encrypt(message.Content),
            Title = _dataProtector.Encrypt(message.Title),
            Recepiants = message.Recepiants.Select(r => _dataProtector.Encrypt(r))
        };

        await dbContext.AddAsync(entityToSave);
        await dbContext.SaveChangesAsync();

        return message;
    }
}
