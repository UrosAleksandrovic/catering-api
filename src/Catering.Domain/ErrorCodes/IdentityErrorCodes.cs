﻿namespace Catering.Domain.ErrorCodes;

public static class IdentityErrorCodes
{
    public const string FORBIDDEN_ACTION = nameof(FORBIDDEN_ACTION);
    public const string INVALID_CREDENTIALS = nameof(INVALID_CREDENTIALS);
    public const string INVITATION_NOT_FOUND = nameof(INVITATION_NOT_FOUND);
    public const string INVITATION_EXPIRED = nameof(INVITATION_EXPIRED);
    public const string IDENTITY_ALREADY_EXISTS = nameof(IDENTITY_ALREADY_EXISTS);
}
