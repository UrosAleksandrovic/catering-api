﻿using Catering.Domain.Aggregates.Identity;

namespace Catering.Domain.Builders;

public class IdentityInvitationBuilder : IBuilder<IdentityInvitation>
{
    public const ushort DefaultDaysToExpire = 30;

    private FullName _fullName;
    private string _email;
    private IdentityRole _futureRole;
    private ushort _daysToExpire = DefaultDaysToExpire;
    private bool _isCustomer;

    public IdentityInvitation Build()
    {
        var result = new IdentityInvitation(_email, _fullName, _daysToExpire, _futureRole, _isCustomer);

        return result;
    }

    public void Reset()
    {
        _fullName = default;
        _email = default;
        _futureRole = default;
        _daysToExpire = DefaultDaysToExpire;
        _isCustomer = default;
    }

    public IdentityInvitationBuilder HasFullName(string firstName, string lastName = null)
    {
        _fullName = new FullName(firstName, lastName);

        return this;
    }

    public IdentityInvitationBuilder HasEmail(string email)
    {
        _email = email;

        return this;
    }

    public IdentityInvitationBuilder HasFutureRole(IdentityRole role, bool isCustomer)
    {
        _futureRole = role;
        _isCustomer = isCustomer;

        return this;
    }
}
