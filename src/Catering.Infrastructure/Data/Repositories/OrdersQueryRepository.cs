using AutoMapper;
using Catering.Application;
using Catering.Application.Aggregates.Orders;
using Catering.Application.Aggregates.Orders.Abstractions;
using Catering.Application.Aggregates.Orders.Dtos;
using Catering.Domain.Aggregates.Order;
using Microsoft.EntityFrameworkCore;

namespace Catering.Infrastructure.Data.Repositories;

internal class OrdersQueryRepository : IOrdersQueryRepository
{
    private readonly IDbContextFactory<CateringDbContext> _dbContextFactory;
    private readonly IMapper _mapper;

    public OrdersQueryRepository(
        IDbContextFactory<CateringDbContext> dbContextFactory,
        IMapper mapper)
    {
        _dbContextFactory = dbContextFactory;
        _mapper = mapper;
    }

    public async Task<OrderInfoDto> GetByIdAsync(long id)
    {
        using var dbContext = _dbContextFactory.CreateDbContext();

        var query = dbContext.Set<Order>().AsQueryable().Where(o => o.Id == id);

        return await _mapper.ProjectTo<OrderInfoDto>(query).SingleOrDefaultAsync();
    }

    public async Task<PageBase<OrderInfoDto>> GetFilteredAsync(OrdersFilter filters)
    {
        using var dbContext = _dbContextFactory.CreateDbContext();
        var filterableOrders = ApplyFilters(filters, dbContext.Set<Order>());
        if (!filters.OrderBy.HasValue)
        {
            var unorderedProjectedQuery = _mapper.ProjectTo<OrderInfoDto>(filterableOrders);
            return new(await unorderedProjectedQuery.ToListAsync(), await filterableOrders.CountAsync());
        }

        filterableOrders = ApplyOrdering(filters, filterableOrders);

        var projectedQuery = _mapper.ProjectTo<OrderInfoDto>(filterableOrders);
        return new(await projectedQuery.ToListAsync(), await filterableOrders.CountAsync());
    }

    public async Task<PageBase<OrderInfoDto>> GetOrdersForCustomerAsync(OrdersFilter filters)
    {
        if (filters.CustomerId == default)
            return PageBase<OrderInfoDto>.Empty();

        return await GetFilteredAsync(filters);
    }

    public async Task<PageBase<OrderInfoDto>> GetOrdersForMenuAsync(OrdersFilter filters)
    {
        if (filters.MenuId == default)
            return PageBase<OrderInfoDto>.Empty();

        return await GetFilteredAsync(filters);
    }

    private IQueryable<Order> ApplyFilters(OrdersFilter filters, IQueryable<Order> query)
    {
        query.AsNoTracking();

        query = query.Include(e => e.Items);

        if (filters.CustomerId != null)
            query = query.Where(o => o.CustomerId == filters.CustomerId);

        if (filters.MenuId != null)
            query = query.Where(o => o.MenuId == filters.MenuId);

        if (filters.TopPrice != null)
            query = query.Where(o => o.Items.Sum(i => i.PriceSnapshot * i.Quantity) <= filters.TopPrice);

        if (filters.BottomPrice != null)
            query = query.Where(o => o.Items.Sum(i => i.PriceSnapshot * i.Quantity) >= filters.BottomPrice);

        if (filters.Statuses != null && filters.Statuses.Count != 0)
            query = query.Where(o => filters.Statuses.Contains(o.Status));

        if (filters.DeliveredOn.HasValue)
            query = query.Where(o => o.ExpectedOn.DayOfYear == filters.DeliveredOn.Value.DayOfYear)
                                             .Where(o => o.ExpectedOn.Year == filters.DeliveredOn.Value.Year);

        if (filters.IsHomeDelivery.HasValue && filters.IsHomeDelivery.Value)
            query = query.Where(o => o.HomeDeliveryInfo != null);

        return query
            .Skip((filters.PageIndex - 1) * filters.PageSize)
            .Take(filters.PageSize);
    }

    private IOrderedQueryable<Order> ApplyOrdering(OrdersFilter filters, IQueryable<Order> query)
    {
        return filters switch
        {
            { OrderBy: OrdersOrderBy.TotalPrice, IsOrderByDescending: false } => query.OrderBy(i => i.TotalPrice),
            { OrderBy: OrdersOrderBy.TotalPrice, IsOrderByDescending: true } => query.OrderByDescending(i => i.TotalPrice),

            { OrderBy: OrdersOrderBy.Status, IsOrderByDescending: false } => query.OrderBy(i => i.Status),
            { OrderBy: OrdersOrderBy.Status, IsOrderByDescending: true } => query.OrderByDescending(i => i.Status),

            { OrderBy: OrdersOrderBy.ExpectedOn, IsOrderByDescending: false } => query.OrderBy(i => i.ExpectedOn),
            { OrderBy: OrdersOrderBy.ExpectedOn, IsOrderByDescending: true } => query.OrderByDescending(i => i.ExpectedOn),

            _ => query.OrderByDescending(i => i.ExpectedOn),
        };
    }
}
