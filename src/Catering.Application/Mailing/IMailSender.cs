namespace Catering.Application.Mailing;

public interface IMailSender<T> where T : IMail
{
    Task<bool> SendAsync(T messageToSend);
}
