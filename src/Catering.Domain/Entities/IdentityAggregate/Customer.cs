﻿namespace Catering.Domain.Entities.IdentityAggregate;

public class Customer : Identity, ICustomer
{
    public CustomerBudget Budget { get; private set; }

    public Customer(
        FullName fullName,
        string email,
        IdentityPermissions permissions)
        : base(fullName, email, permissions)
    {
        Budget = new CustomerBudget(0);
    }

    public Customer(string email, IdentityPermissions permissions)
        :base(null, email, permissions)
    {
        Budget = new CustomerBudget(0);
    }

    public void ResetBudget(decimal newBalance)
        => Budget.SetBalance(newBalance);

    public void ReserveAssets(decimal amountToReserve)
        => Budget.Reserve(amountToReserve);

    public void ProcessPayment(decimal amountToPay)
        => Budget.Remove(amountToPay);

    public void CancelPayment(decimal amountToCancel)
        => Budget.CancelReservation(amountToCancel);
}