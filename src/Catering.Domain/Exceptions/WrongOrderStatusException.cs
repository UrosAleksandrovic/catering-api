namespace Catering.Domain.Exceptions;

[Serializable]
public class WrongOrderStatusException(string action, long orderId) 
    : CateringException($"Order ({orderId}) cannot perform action ({action}) because it is not in correct status")
{ }
