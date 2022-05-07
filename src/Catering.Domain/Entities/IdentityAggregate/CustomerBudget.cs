using Ardalis.GuardClauses;

namespace Catering.Domain.Entities.IdentityAggregate;

public class CustomerBudget
{
    public decimal Balance { get; private set; }
    public decimal ReservedAssets { get; private set; }

    private CustomerBudget() { }

    public CustomerBudget(decimal balance)
    {
        Guard.Against.Negative(balance);

        Balance = balance;
    }

    public void SetBalance(decimal newBalance)
    {
        Guard.Against.Negative(newBalance);

        Balance = newBalance;
    }

    public void Reserve(decimal amountToReserve)
    {
        Guard.Against.Negative(amountToReserve);

        ReservedAssets += amountToReserve;
    }

    public void CancelReservation(decimal amountToCancel)
    {
        Guard.Against.Negative(amountToCancel);
        Guard.Against.OutOfRange(amountToCancel, nameof(amountToCancel), 0, ReservedAssets);

        ReservedAssets -= amountToCancel;
    }

    public void Remove(decimal amountToRemove)
    {
        Guard.Against.Negative(amountToRemove);
        Guard.Against.OutOfRange(amountToRemove, nameof(amountToRemove), 0, ReservedAssets);

        ReservedAssets -= amountToRemove;
        Balance -= amountToRemove;
    }
}
