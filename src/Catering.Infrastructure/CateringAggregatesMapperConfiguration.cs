﻿using AutoMapper;
using Catering.Application.Aggregates.Carts.Dtos;
using Catering.Application.Aggregates.Expenses.Dtos;
using Catering.Application.Aggregates.Identities.Dtos;
using Catering.Application.Aggregates.Items.Dtos;
using Catering.Application.Aggregates.Menus.Dtos;
using Catering.Application.Aggregates.Orders.Dtos;
using Catering.Application.Dtos.Menu;
using Catering.Domain.Aggregates.Cart;
using Catering.Domain.Aggregates.Expense;
using Catering.Domain.Aggregates.Identity;
using Catering.Domain.Aggregates.Item;
using Catering.Domain.Aggregates.Menu;
using Catering.Domain.Aggregates.Order;

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
            .ForMember(d => d.Role, opt => opt.MapFrom(s => s.Role.ToString()));

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
            .ForMember(d => d.TotalSumToPay, opt => opt.MapFrom(s => s.TotalPrice))
            .ForMember(d => d.OrderedAt, opt => opt.MapFrom(s => s.CreatedOn));

        CreateMap<Order, ListOrderInfoDto>()
            .ForMember(d => d.TotalSumToPay, opt => opt.MapFrom(s => s.TotalPrice))
            .ForMember(d => d.IsHomeDelivery, opt => opt.MapFrom(s => s.IsForHomeDelivery));

        CreateMap<Expense, ExpenseInfoDto>();
    }
}
