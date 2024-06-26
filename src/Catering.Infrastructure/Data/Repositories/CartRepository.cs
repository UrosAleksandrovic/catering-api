﻿using Catering.Application.Aggregates.Carts.Abstractions;
using Catering.Domain.Aggregates.Cart;
using Microsoft.EntityFrameworkCore;

namespace Catering.Infrastructure.Data.Repositories;

internal class CartRepository : BaseCrudRepository<Cart, CateringDbContext>, ICartRepository
{
    public CartRepository(CateringDbContext dbContext) : base(dbContext)
    { }

    public Task DeleteByCustomerIdAsync(string customerId)
        => _dbContext.Carts
            .Where(c => c.CustomerId == customerId)
            .ExecuteDeleteAsync();

    public Task DeleteItemsWithMenuAsync(Guid menuId)
        => _dbContext.Carts
            .Where(c => c.MenuId == menuId)
            .ExecuteDeleteAsync();

    public Task<Cart> GetByCustomerIdAsync(string customerId)
        => _dbContext.Carts
            .SingleOrDefaultAsync(c => c.CustomerId == customerId);
}
