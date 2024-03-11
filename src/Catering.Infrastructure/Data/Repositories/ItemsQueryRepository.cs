using AutoMapper;
using Catering.Application;
using Catering.Application.Aggregates.Items;
using Catering.Application.Aggregates.Items.Abstractions;
using Catering.Application.Aggregates.Items.Dtos;
using Catering.Domain.Aggregates.Item;
using Catering.Domain.Aggregates.Order;
using Catering.Infrastructure.EFUtility;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Query.Internal;

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

        var queryableItems = dbContext.Items.AsNoTracking();
        queryableItems = ApplyFilters(filters, queryableItems);
        queryableItems = queryableItems.ApplyOrdering(filters.OrderBy, filters.IsOrderByDescending);

        var projectedQuery = _mapper.ProjectTo<ItemInfoDto>(queryableItems);

        return new (await projectedQuery.Paginate(filters).ToListAsync(), await queryableItems.CountAsync());
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

        return queryableItems;
    }
}
