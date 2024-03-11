using Catering.Domain.ErrorCodes;

namespace Catering.Domain.Exceptions;

[Serializable]
public class IdentityRestrictionException: CateringException
{
    private const string CustomMessage = "Identity ({identityId}) is not allowed to perform action ({actionName}) based on persmissions.";

    public IdentityRestrictionException() : base(IdentityErrorCodes.FORBIDDEN_ACTION) { }

    public IdentityRestrictionException(string identityId, string actionName)
        : base(IdentityErrorCodes.FORBIDDEN_ACTION, CustomMessage)
    {
        Data.Add(nameof(identityId), identityId);
        Data.Add(nameof(actionName), actionName);
    }
}
