namespace Catering.Domain.Exceptions;

[Serializable]
public class IdentityRestrictionException : CateringException
{
    public IdentityRestrictionException(string identityId, string actionName) 
        : base($"Identity ({identityId}) is not allowed to perform action ({actionName}) based on persmissions.") 
    { }
}
