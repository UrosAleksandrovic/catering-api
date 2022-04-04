﻿using Catering.Domain.Entities.ItemAggregate;

namespace Catering.Domain.Abstractions;

public interface IItemRepository
{
    Task<Item> GetByIdAsync(Guid id);
    Task AddAsync(Item item);
    Task UpdateAsync(Item item);
    Task DeleteAsync(Guid id);
    Task<List<Item>> GetItemsFromMenuAsync(Guid menuId);
}
