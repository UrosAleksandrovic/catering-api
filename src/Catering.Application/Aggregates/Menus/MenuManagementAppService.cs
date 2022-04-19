using AutoMapper;
using Catering.Application.Aggregates.Menus.Abstractions;
using Catering.Application.Aggregates.Menus.Dtos;
using Catering.Application.Aggregates.Menus.Requests;
using Catering.Application.Dtos.Menu;
using Catering.Domain.Builders;
using MediatR;

namespace Catering.Application.Aggregates.Menus;

internal class MenuManagementAppService : IMenuManagementAppService
{
    private readonly IMenuRepository _menuRepository;
    private readonly IMapper _mapper;
    private readonly IMediator _publisher;

    public MenuManagementAppService(
        IMenuRepository menuRepository,
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

        await _publisher.Publish(new MenuDeleted { MenuId = menu.Id });
    }

    public async Task<MenuInfoDto> GetByIdAsync(Guid id)
    {
        return _mapper.Map<MenuInfoDto>(await _menuRepository.GetByIdAsync(id));
    }

    public async Task<Guid> UpdateAsync(Guid id, CreateMenuDto updateMenu)
    {
        var menu = await _menuRepository.GetByIdAsync(id);
        if (menu == default)
            throw new KeyNotFoundException();

        menu.AddOrEditContact(updateMenu.PhoneNumber, updateMenu.Email, updateMenu.Address);
        menu.Edit(updateMenu.Name);

        await _menuRepository.UpdateAsync(menu);

        return menu.Id;
    }
}
