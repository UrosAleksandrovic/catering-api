﻿using Ardalis.GuardClauses;
using Catering.Domain.Aggregates.Cart;
using Catering.Domain.Aggregates.Item;
using Catering.Domain.Aggregates.Order;
using Catering.Domain.Exceptions;

namespace Catering.Domain.Builders;

public class OrderBuilder : IBuilder<Order>
{
    private string _customerId;
    private DateTimeOffset _expectedOn;
    private HomeDeliveryInfo _homeDeliveryInfo;
    private Cart _cart;
    private IEnumerable<Item> _items;

    public Order Build()
    {
        var menuId = GetMenuId();
        var orderItems = GenerateOrderItems();

        return new Order(orderItems, menuId, _customerId, _expectedOn, _homeDeliveryInfo);
    }

    public void Reset()
    {
        _customerId = default;
        _expectedOn = default;
        _homeDeliveryInfo = default;
        _cart = default;
        _items = default;
    }

    public OrderBuilder HasDateOfDelivery(DateTimeOffset expectedOn)
    {
        _expectedOn = expectedOn;

        return this;
    }

    public OrderBuilder HasHomeDeliveryOption(string streetAndHouse, string floorAndAppartment)
    {
        _homeDeliveryInfo = new HomeDeliveryInfo(streetAndHouse, floorAndAppartment);

        return this;
    }

    public OrderBuilder HasCart(Cart cart)
    {
        _cart = cart;
        _customerId = cart.CustomerId;

        return this;
    }

    public OrderBuilder HasItems(IEnumerable<Item> items)
    {
        _items = items;

        return this;
    }

    private Guid GetMenuId()
    {
        Guard.Against.NullOrEmpty(_items);

        var itemMenus = _items.Select(i => i.MenuId);
        var firstItemMenu = itemMenus.First();
        if (!itemMenus.All(i => i == firstItemMenu))
            throw new ItemMenuNotValidException();

        return firstItemMenu;
    }

    private List<OrderItem> GenerateOrderItems()
    {
        List<OrderItem> orderItems = [];

        foreach (var item in _items)
        {
            var cartItem = _cart.Items.SingleOrDefault(c => c.ItemId == item.Id);

            if (cartItem == null)
                throw new ItemNotInCartException(_cart.Id, item.Id);

            orderItems.Add(new OrderItem(item.Id, item.Price, item.Name, cartItem.Quantity, cartItem.Note));
        }

        return orderItems;
    }
}
