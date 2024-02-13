
namespace Catering.Domain.Exceptions;

[Serializable]
public class IdentityRestrictionException(string identityId, string actionName) 
    : CateringException($"Identity ({identityId}) is not allowed to perform action ({actionName}) based on persmissions.")
{ }
