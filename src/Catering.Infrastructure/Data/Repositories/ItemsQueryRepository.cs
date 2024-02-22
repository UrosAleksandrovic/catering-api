using AutoMapper;
using Catering.Application;
using Catering.Application.Aggregates.Items;
using Catering.Application.Aggregates.Items.Abstractions;
using Catering.Application.Aggregates.Items.Dtos;
using Catering.Domain.Aggregates.Item;
using Catering.Domain.Aggregates.Order;
using Microsoft.EntityFrameworkCore;

namespace Catering.Infrastructure.Data.Repositories;

internal class ItemsQueryRepository : IItemsQueryRepository
{
    private readonly IDbContextFactory<CateringDbContext> _dbContextFactory;
    private readonly IMapper _mapper;

    public ItemsQueryRepository(
        IDbContextFactory<CateringDbContext> dbContextFactory,
        IMapper mapper)
    {
        _dbContextFactory = dbContextFactory;
        _mapper = mapper;
    }

    public async Task<List<ItemsLeaderboardDto>> GetMostOrderedFromTheMenuAsync(Guid menuId, int top)
    {
        var dbContext = _dbContextFactory.CreateDbContext();

        return await dbContext.Set<Order>()
            .AsNoTracking()
            .Where(o => o.MenuId == menuId)
            .SelectMany(o => o.Items)
            .GroupBy(i => new { i.ItemId, i.NameSnapshot})
            .Select(g => new ItemsLeaderboardDto(g.Key.ItemId, g.Key.NameSnapshot, g.Sum(i => i.Quantity)))
            .ToListAsync();
    }

    public async Task<PageBase<ItemInfoDto>> GetPageAsync(ItemsFilter filters)
    {
        var dbContext = _dbContextFactory.CreateDbContext();

        var queryableItems = dbContext.Set<Item>().AsQueryable();
        queryableItems = ApplyFilters(filters, queryableItems);

        if (!filters.OrderBy.HasValue)
        {
            var unorderedProjectedQuery = _mapper.ProjectTo<ItemInfoDto>(queryableItems);
            return new (await unorderedProjectedQuery.ToListAsync(), await queryableItems.CountAsync());
        }

        var orderedQueryable = ApplyOrdering(filters, queryableItems);
        var projectedQuery = _mapper.ProjectTo<ItemInfoDto>(orderedQueryable);

        return new (await projectedQuery.ToListAsync(), await orderedQueryable.CountAsync());
    }

    private IQueryable<Item> ApplyFilters(ItemsFilter itemsFilter, IQueryable<Item> queryableItems)
    {
        queryableItems
            .AsNoTracking()
            .Include(i => i.Ingredients)
            .Include(i => i.Categories);

        if (itemsFilter.Categories != null && itemsFilter.Categories.Any())
            foreach (var category in itemsFilter.Categories)
                queryableItems = queryableItems.Where(i => i.Categories.Any(c => c.Id == category));

        if (itemsFilter.Ingredients != null && itemsFilter.Ingredients.Any())
            foreach (var ingredient in itemsFilter.Ingredients)
                queryableItems = queryableItems.Where(i => i.Ingredients.Any(c => c.Id == ingredient));

        if (itemsFilter.TopPrice != null)
            queryableItems = queryableItems.Where(i => i.Price <= itemsFilter.TopPrice);

        if (itemsFilter.BottomPrice != null)
            queryableItems = queryableItems.Where(i => i.Price >= itemsFilter.BottomPrice);

        queryableItems = queryableItems.Where(i => i.MenuId == itemsFilter.MenuId);

        return queryableItems
            .Skip((itemsFilter.PageIndex - 1) * itemsFilter.PageSize)
            .Take(itemsFilter.PageSize);
    }

    private IOrderedQueryable<Item> ApplyOrdering(ItemsFilter itemsFilter, IQueryable<Item> queryableItems)
    {
        return itemsFilter switch
        {
            { OrderBy: ItemsOrderBy.Price, IsOrderByDescending: false } => queryableItems.OrderBy(i => i.Price),
            { OrderBy: ItemsOrderBy.Price, IsOrderByDescending: true } => queryableItems.OrderByDescending(i => i.Price),

            { OrderBy: ItemsOrderBy.Name, IsOrderByDescending: false } => queryableItems.OrderBy(i => i.Name),
            { OrderBy: ItemsOrderBy.Name, IsOrderByDescending: true } => queryableItems.OrderByDescending(i => i.Name),

            { OrderBy: ItemsOrderBy.Rating, IsOrderByDescending: false } => queryableItems.OrderBy(i => i.Ratings.Average(r => r.Rating)),
            { OrderBy: ItemsOrderBy.Rating, IsOrderByDescending: true } => queryableItems.OrderByDescending(i => i.Ratings.Average(r => r.Rating)),

            _ => queryableItems.OrderBy(i => i.Name),
        };
    }
}
