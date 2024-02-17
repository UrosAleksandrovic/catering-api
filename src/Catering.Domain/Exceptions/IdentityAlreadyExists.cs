using Catering.Domain.ErrorCodes;

namespace Catering.Domain.Exceptions;

public class IdentityAlreadyExists : CateringException
{
    public IdentityAlreadyExists() : base(IdentityErrorCodes.IDENTITY_ALREADY_EXISTS) { }
}
