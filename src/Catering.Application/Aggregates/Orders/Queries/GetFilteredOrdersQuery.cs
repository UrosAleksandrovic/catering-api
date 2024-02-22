using Catering.Application.Aggregates.Orders.Abstractions;
using Catering.Application.Aggregates.Orders.Dtos;
using Catering.Application.Aggregates.Orders.Requests;
using Catering.Domain.Aggregates.Identity;
using Catering.Domain.Aggregates.Order;
using MediatR;

namespace Catering.Application.Aggregates.Orders.Queries;

public record GetFilteredOrdersQuery(OrdersFilter Filters, string RequestorId) : IRequest<FilterResult<OrderInfoDto>>;

internal class GetFilteredOrdersQueryHandler(IOrdersQueryRepository queryRepository, IMediator publisher)
    : IRequestHandler<GetFilteredOrdersQuery, FilterResult<OrderInfoDto>>
{
    private readonly IOrdersQueryRepository _queryRepository = queryRepository;
    private readonly IMediator _publisher = publisher;

    public async Task<FilterResult<OrderInfoDto>> Handle(
        GetFilteredOrdersQuery request,
        CancellationToken cancellationToken)
    {
        var orders = await GetFilteredOrdersBasedOnRequestorAsync(request.Filters, request.RequestorId);

        return new FilterResult<OrderInfoDto>
        {
            PageIndex = request.Filters.PageIndex,
            PageSize = request.Filters.PageSize,
            Result = orders.Data,
            TotalNumberOfElements = orders.TotalCount,
        };
    }

    private async Task<PageBase<OrderInfoDto>> GetFilteredOrdersBasedOnRequestorAsync(
        OrdersFilter orderFilters,
        string requestorId)
    {
        var identityRequest = new GetIdentityWithId { IdentityId = requestorId };
        var requestorIdentity = await _publisher.Send(identityRequest);

        if (requestorIdentity.Role.IsClientEmployee() && !requestorIdentity.Role.IsAdministrator())
        {
            var newFilter = new OrdersFilter(orderFilters)
            {
                CustomerId = requestorIdentity.Id
            };
            return await _queryRepository.GetOrdersForCustomerAsync(newFilter);
        }

        if (requestorIdentity.Role.IsRestaurantEmployee())
        {
            var newFilter = new OrdersFilter(orderFilters)
            {
                MenuId = await _publisher.Send(new GetMenuWithContactId { ContactId = requestorId })
            };
            return await _queryRepository.GetOrdersForMenuAsync(newFilter);
        }

        return await _queryRepository.GetFilteredAsync(orderFilters);
    }
}
