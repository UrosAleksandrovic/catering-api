﻿namespace Catering.Domain.Aggregates.Identity;

public class Customer : ICustomer
{
    public string IdentityId { get; private set; }
    public Identity Identity { get; private set; }

    public CustomerBudget Budget { get; private set; }

    protected Customer() : base() { }

    public Customer(Identity identity)
    {
        IdentityId = identity.Id;
        Budget = new CustomerBudget(0);
    }

    public void ResetBudget(decimal newBalance)
        => Budget.SetBalance(newBalance);

    public void ReserveAssets(decimal amountToReserve)
        => Budget.Reserve(amountToReserve);

    public void ProcessPayment(decimal amountToPay)
        => Budget.Remove(amountToPay);

    public void ProcessExpense(decimal amountToPay)
        => Budget.RemoveDirect(amountToPay);

    public void RevertPayment(decimal amountToPlay)
        => Budget.Add(amountToPlay);

    public void CancelPayment(decimal amountToCancel)
        => Budget.CancelReservation(amountToCancel);
}
