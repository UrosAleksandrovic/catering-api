namespace Catering.Domain.Exceptions;

[Serializable]
public class InvalidCredentialsException : CateringException
{
    public InvalidCredentialsException(string username, Exception innerException)
        : base(GetDefaultMessage(username), innerException)
    {
    }

    public InvalidCredentialsException(string username)
        : base(GetDefaultMessage(username))
    { }

    private static string GetDefaultMessage(string username) 
        => $"Credentials for the username ${username} was incorect.";
}
