using Catering.Domain.ErrorCodes;

namespace Catering.Domain.Exceptions;

[Serializable]
public class InvalidCredentialsException : CateringException
{
    private const string CustomMessage = "Credentials for the username {username} was incorect.";

    public InvalidCredentialsException(string username, Exception innerException)
        : base(IdentityErrorCodes.INVALID_CREDENTIALS, CustomMessage, innerException)
    {
        Data.Add(nameof(username), username);
    }

    public InvalidCredentialsException(string username)
        : base(IdentityErrorCodes.INVALID_CREDENTIALS, CustomMessage)
    {
        Data.Add(nameof(username), username);
    }
}
