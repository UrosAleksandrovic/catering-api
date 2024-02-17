namespace Catering.Domain.Exceptions;

[Serializable]
public class WrongOrderStatusException : CateringException
{
    public const string CustomMessage = "Order ({orderId}) cannot perform action ({action}) because it is not in correct status";

    public WrongOrderStatusException(string action, long orderId)
    {
        Data.Add(nameof(orderId), orderId);
        Data.Add(nameof(action), action);
    }
}
