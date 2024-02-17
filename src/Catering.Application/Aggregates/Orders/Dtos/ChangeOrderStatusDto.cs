using Catering.Domain.Aggregates.Order;

namespace Catering.Application.Aggregates.Orders.Dtos;

public class ChangeOrderStatusDto
{
    public OrderStatus NewStatus { get; set; }
}
