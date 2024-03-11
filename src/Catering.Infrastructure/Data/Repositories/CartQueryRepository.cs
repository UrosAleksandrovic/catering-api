using AutoMapper;
using Catering.Application.Aggregates.Carts.Abstractions;
using Catering.Application.Aggregates.Carts.Dtos;
using Microsoft.EntityFrameworkCore;

namespace Catering.Infrastructure.Data.Repositories;

internal class CartQueryRepository : ICartQueryRepository
{
    private readonly IDbContextFactory<CateringDbContext> _dbContextFactory;
    private readonly IMapper _mapper;

    public CartQueryRepository(
        IDbContextFactory<CateringDbContext> dbContextFactory,
        IMapper mapper)
    {
        _dbContextFactory = dbContextFactory;
        _mapper = mapper;
    }

    public async Task<CartInfoDto> GetByCustomerIdAsync(string customerId)
    {
        using var dbContext = await _dbContextFactory.CreateDbContextAsync();

        var query = dbContext.Carts.Where(x => x.CustomerId == customerId);

        return await _mapper.ProjectTo<CartInfoDto>(query).SingleOrDefaultAsync();
    }

    public async Task<bool> ExistsAsync(string customerId)
    {
        using var dbContext = await _dbContextFactory.CreateDbContextAsync();

        var query = dbContext.Carts.Where(x => x.CustomerId == customerId);

        return await _mapper.ProjectTo<CartInfoDto>(query).AnyAsync();
    }
}
