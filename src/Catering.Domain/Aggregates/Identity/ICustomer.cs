namespace Catering.Domain.Aggregates.Identity;

public interface ICustomer
{
    CustomerBudget Budget { get; }

    void ResetBudget(decimal newBalance);

    void ReserveAssets(decimal amountToReserve);

    void ProcessPayment(decimal amountToPay);

    void CancelPayment(decimal amountToCancel);
}
