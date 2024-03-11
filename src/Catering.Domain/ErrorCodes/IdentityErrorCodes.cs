namespace Catering.Domain.ErrorCodes;

public static class IdentityErrorCodes
{
    public const string FORBIDDEN_ACTION = nameof(FORBIDDEN_ACTION);
    public const string INVALID_CREDENTIALS = nameof(INVALID_CREDENTIALS);
    public const string INVITATION_NOT_FOUND = nameof(INVITATION_NOT_FOUND);
    public const string INVITATION_EXPIRED = nameof(INVITATION_EXPIRED);
    public const string IDENTITY_ALREADY_EXISTS = nameof(IDENTITY_ALREADY_EXISTS);
    public const string INVALID_CUSTOMER_ID = nameof(INVALID_CUSTOMER_ID);
    public const string INVALID_CREATOR_ROLE = nameof(INVALID_CREATOR_ROLE);
    public const string INITIATOR_IDENTITY_NOT_FOUND = nameof(INITIATOR_IDENTITY_NOT_FOUND);
}
