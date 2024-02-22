using AutoMapper;
using Catering.Application.Aggregates.Menus.Abstractions;
using Catering.Application.Aggregates.Menus.Dtos;
using Catering.Application.Aggregates.Menus.Notifications;
using Catering.Application.Aggregates.Menus.Requests;
using Catering.Application.Dtos.Menu;
using Catering.Domain.Aggregates.Identity;
using Catering.Domain.Builders;
using MediatR;

namespace Catering.Application.Aggregates.Menus;

internal class MenuManagementAppService : IMenusManagementAppService
{
    private readonly IMenusRepository _menuRepository;
    private readonly IMapper _mapper;
    private readonly IMediator _publisher;

    public MenuManagementAppService(
        IMenusRepository menuRepository,
        IMapper mapper,
        IMediator publisher)
    {
        _menuRepository = menuRepository;
        _mapper = mapper;
        _publisher = publisher;
    }

    public async Task<Guid> CreateAsync(CreateMenuDto createMenu)
    {
        var menuToCreate = new MenuBuilder()
            .HasName(createMenu.Name)
            .HasContact(createMenu.PhoneNumber, createMenu.Email, createMenu.Address)
            .HasContactIdentity(createMenu.ContactIdentityId)
            .Build();

        var createdItem = await _menuRepository.CreateAsync(menuToCreate);

        return createdItem.Id;
    }

    public async Task DeleteAsync(Guid id)
    {
        var menu = await _menuRepository.GetByIdAsync(id);
        if (menu == default)
            throw new KeyNotFoundException();

        menu.MarkAsDeleted();
        await _menuRepository.UpdateAsync(menu);

        await _publisher.Publish(new MenuDeleted(menu.Id));
    }

    public async Task<MenuInfoDto> GetByIdAsync(Guid id, string requestorId)
    {
        var menu = await _menuRepository.GetByIdAsync(id);

        var requestor = await _publisher.Send(new GetIdentityById { Id = requestorId });

        if (!requestor.Role.IsRestaurantEmployee())
            return _mapper.Map<MenuInfoDto>(menu);

        return menu.HasContact(requestorId) ? _mapper.Map<MenuInfoDto>(menu) : null;
    }

    public async Task UpdateAsync(Guid id, UpdateMenuDto updateMenu)
    {
        var menu = await _menuRepository.GetByIdAsync(id);
        if (menu == default)
            throw new KeyNotFoundException();

        menu.AddOrEditContact(updateMenu.PhoneNumber,
                              updateMenu.Email,
                              updateMenu.Address);
        menu.Edit(updateMenu.Name);

        await _menuRepository.UpdateAsync(menu);
    }
}
