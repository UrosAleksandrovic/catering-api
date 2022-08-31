using AutoMapper;
using Catering.Application.Aggregates.Carts.Dtos;
using Catering.Application.Aggregates.Identites.Dtos;
using Catering.Application.Aggregates.Items.Dtos;
using Catering.Application.Aggregates.Menus.Dtos;
using Catering.Application.Aggregates.Orders.Dtos;
using Catering.Application.Dtos.Menu;
using Catering.Domain.Entities.CartAggregate;
using Catering.Domain.Entities.IdentityAggregate;
using Catering.Domain.Entities.ItemAggregate;
using Catering.Domain.Entities.MenuAggregate;
using Catering.Domain.Entities.OrderAggregate;

namespace Catering.Infrastructure;

internal class CateringAggregatesMapperConfiguration : Profile
{
    public CateringAggregatesMapperConfiguration()
    {
        CreateMap<CartItem, CartItemInfoDto>()
            .ForMember(c => c.Price, opt => opt.Ignore())
            .ForMember(c => c.Name, opt => opt.Ignore())
            .ForMember(c => c.Description, opt => opt.Ignore());

        CreateMap<Cart, CartInfoDto>()
            .ForMember(c => c.Items, opt => opt.MapFrom(s => s.Items));

        CreateMap<Identity, IdentityInfoDto>()
            .ForMember(d => d.FirstName, opt => opt.MapFrom(s => s.FullName.FirstName))
            .ForMember(d => d.LastName, opt => opt.MapFrom(s => s.FullName.LastName))
            .ForMember(d => d.Roles, opt => opt.MapFrom(s => s.Role.GetRoles().Select(r => r.ToString())));

        CreateMap<CustomerBudget, CustomerBudgetInfoDto>()
            .ForMember(c => c.ReservedBalance, opt => opt.MapFrom(s => s.ReservedAssets))
            .ForMember(d => d.Budget, opt => opt.MapFrom(s => s.Balance));

        CreateMap<Customer, CustomerInfoDto>()
            .ForMember(d => d.CustomerBudgetInfo, opt => opt.MapFrom(s => s.Budget))
            .ForMember(d => d.IdentityInfo, opt => opt.MapFrom(s => s.Identity));

        CreateMap<Item, ItemInfoDto>()
            .ForMember(d => d.TotalRating, opt => opt.MapFrom(s => s.TotalRating))
            .ForMember(d => d.Ingredients, opt => opt.MapFrom(s => s.Ingredients.Select(i => i.DisplayName)))
            .ForMember(d => d.Categories, opt => opt.MapFrom(s => s.Categories.Select(c => c.DisplayName)));

        CreateMap<MenuContact, MenuContactInfoDto>();
        CreateMap<Menu, MenuInfoDto>();

        CreateMap<OrderItem, OrderItemInfoDto>();
        CreateMap<HomeDeliveryInfo, HomeDeliveryInfoDto>();
        CreateMap<Order, OrderInfoDto>()
            .ForMember(d => d.Items, opt => opt.MapFrom(s => s.Items))
            .ForMember(d => d.TotalSumToPay, opt => opt.MapFrom(s => s.TotalPrice));
    }
}
