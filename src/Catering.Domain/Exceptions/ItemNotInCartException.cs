﻿namespace Catering.Domain.Exceptions;

[Serializable]
public class ItemNotInCartException : CateringException
{
    public ItemNotInCartException(Guid cartId, Guid itemId)
        : base($"Item ({itemId}) not found in ({cartId}), therefore it cannot be modified.") { }
}
