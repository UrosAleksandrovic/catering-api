
namespace Catering.Domain.Exceptions;

[Serializable]
public class IdentityRestrictionException : CateringException
{
    public IdentityRestrictionException(string identityId, string actionName)
        : base($"Identity ({identityId}) is not allowed to perform action ({actionName}) based on persmissions.")
    { }

    public IdentityRestrictionException()
    { }

    public IdentityRestrictionException(string message) : base(message)
    { }

    public IdentityRestrictionException(string message, Exception innerException) : base(message, innerException)
    { }
}
