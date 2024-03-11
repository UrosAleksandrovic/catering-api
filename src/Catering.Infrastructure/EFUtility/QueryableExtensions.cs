using Catering.Application.Aggregates.Expenses;
using Catering.Application.Aggregates.Items;
using Catering.Application.Aggregates.Orders;
using Catering.Application.Filtering;
using Catering.Domain.Aggregates.Expense;
using Catering.Domain.Aggregates.Item;
using Catering.Domain.Aggregates.Order;

namespace Catering.Infrastructure.EFUtility;

internal static class QueryableExtensions
{
    public static IQueryable<T> Paginate<T>(this IQueryable<T> query, FilterBase filterBase)
        => query.Skip((filterBase.PageIndex - 1) * filterBase.PageSize).Take(filterBase.PageSize);

    public static IQueryable<Item> ApplyOrdering(
        this IQueryable<Item> query,
        ItemsOrderBy? orderBy,
        bool? isDescending) 
    {
        if (orderBy == null)
            return query.OrderBy(i => i.Name);

        var orderInfo = new OrderByInfo<ItemsOrderBy>(orderBy.Value, isDescending ?? false);

        return orderInfo switch
        {
            { OrderBy: ItemsOrderBy.Price, IsDescending: false } => query.OrderBy(i => i.Price),
            { OrderBy: ItemsOrderBy.Price, IsDescending: true } => query.OrderByDescending(i => i.Price),

            { OrderBy: ItemsOrderBy.Name, IsDescending: false } => query.OrderBy(i => i.Name),
            { OrderBy: ItemsOrderBy.Name, IsDescending: true } => query.OrderByDescending(i => i.Name),

            { OrderBy: ItemsOrderBy.Rating, IsDescending: false } => query.OrderBy(i => i.Ratings.Average(r => r.Rating)),
            { OrderBy: ItemsOrderBy.Rating, IsDescending: true } => query.OrderByDescending(i => i.Ratings.Average(r => r.Rating)),

            _ => query.OrderBy(i => i.Name),
        };
    }

    public static IQueryable<Expense> ApplyOrdering(
        this IQueryable<Expense> query,
        ExpensesOrderBy? orderBy,
        bool? isDescending)
    {
        if (orderBy == null)
            return query.OrderByDescending(i => i.DeliveredOn);

        var orderInfo = new OrderByInfo<ExpensesOrderBy>(orderBy.Value, isDescending ?? false);

        return orderInfo switch
        {
            { OrderBy: ExpensesOrderBy.Price, IsDescending: false } => query.OrderBy(i => i.Price),
            { OrderBy: ExpensesOrderBy.Price, IsDescending: true } => query.OrderByDescending(i => i.Price),

            { OrderBy: ExpensesOrderBy.DeliveredOn, IsDescending: false } => query.OrderBy(i => i.DeliveredOn),
            { OrderBy: ExpensesOrderBy.DeliveredOn, IsDescending: true } => query.OrderByDescending(i => i.DeliveredOn),

            _ => query.OrderByDescending(i => i.DeliveredOn),
        };
    }

    public static IQueryable<Order> ApplyOrdering(
        this IQueryable<Order> query,
        OrdersOrderBy? orderBy,
        bool? isDescending)
    {
        if (orderBy == null)
            return query.OrderByDescending(i => i.ExpectedOn);

        var orderInfo = new OrderByInfo<OrdersOrderBy>(orderBy.Value, isDescending ?? false);

        return orderInfo switch
        {
            { OrderBy: OrdersOrderBy.TotalPrice, IsDescending: false } => query.OrderBy(i => i.TotalPrice),
            { OrderBy: OrdersOrderBy.TotalPrice, IsDescending: true } => query.OrderByDescending(i => i.TotalPrice),

            { OrderBy: OrdersOrderBy.Status, IsDescending: false } => query.OrderBy(i => i.Status),
            { OrderBy: OrdersOrderBy.Status, IsDescending: true } => query.OrderByDescending(i => i.Status),

            { OrderBy: OrdersOrderBy.ExpectedOn, IsDescending: false } => query.OrderBy(i => i.ExpectedOn),
            { OrderBy: OrdersOrderBy.ExpectedOn, IsDescending: true } => query.OrderByDescending(i => i.ExpectedOn),

            _ => query.OrderByDescending(i => i.ExpectedOn),
        };
    }
}

internal record OrderByInfo<TOrderBy>(TOrderBy OrderBy, bool IsDescending) where TOrderBy : Enum;
