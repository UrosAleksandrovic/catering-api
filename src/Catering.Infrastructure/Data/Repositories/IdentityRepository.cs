﻿using Catering.Application.Aggregates.Identites.Abstractions;
using Catering.Domain.Entities.IdentityAggregate;
using Microsoft.EntityFrameworkCore;

namespace Catering.Infrastructure.Data.Repositories;

internal class IdentityRepository<T> : BaseRepository<T, CateringDbContext>, IIdentityRepository<T> 
    where T : Identity
{
    protected IdentityRepository(IDbContextFactory<CateringDbContext> dbContextFactory) 
        : base(dbContextFactory) { }

    public async Task<T> GetByEmailAsync(string email)
    {
        using var dbContext = await _dbContextFactory.CreateDbContextAsync();

        var result = await dbContext.Set<T>().FirstOrDefaultAsync(e => e.Email == email);

        return result;
    }
}