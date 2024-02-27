using AutoMapper;
using Catering.Application.Aggregates.Menus.Abstractions;
using Catering.Application.Aggregates.Menus.Dtos;
using Catering.Application.Aggregates.Menus.Notifications;
using Catering.Application.Aggregates.Menus.Requests;
using Catering.Application.Dtos.Menu;
using Catering.Application.Results;
using Catering.Application.Validation;
using Catering.Domain.Aggregates.Identity;
using Catering.Domain.Builders;
using MediatR;

namespace Catering.Application.Aggregates.Menus;

internal class MenuManagementAppService : IMenusManagementAppService
{
    private readonly IMenusRepository _menuRepository;
    private readonly IValidationProvider _validationProvider;
    private readonly IMapper _mapper;
    private readonly IMediator _publisher;

    public MenuManagementAppService(
        IMenusRepository menuRepository,
        IValidationProvider validationProvider,
        IMapper mapper,
        IMediator publisher)
    {
        _menuRepository = menuRepository;
        _validationProvider = validationProvider;
        _mapper = mapper;
        _publisher = publisher;
    }

    public async Task<Result<Guid>> CreateAsync(CreateMenuDto createRequest)
    {
        if (await _validationProvider.ValidateModelAsync(createRequest) is var valRes && !valRes.IsSuccess)
            return Result.From<Guid>(valRes);

        var menuToCreate = new MenuBuilder()
            .HasName(createRequest.Name)
            .HasContact(createRequest.PhoneNumber, createRequest.Email, createRequest.Address)
            .HasContactIdentity(createRequest.ContactIdentityId)
            .Build();

        var createdItem = await _menuRepository.CreateAsync(menuToCreate);

        return Result.Success(createdItem.Id);
    }

    public async Task<Result> DeleteAsync(Guid id)
    {
        var menu = await _menuRepository.GetByIdAsync(id);
        if (menu == default)
            return Result.NotFound();

        menu.MarkAsDeleted();
        await _menuRepository.UpdateAsync(menu);

        await _publisher.Publish(new MenuDeleted(menu.Id));
        return Result.Success();
    }

    //TODO: Should I transition to Query?
    public async Task<Result<MenuInfoDto>> GetByIdAsync(Guid id, string requestorId)
    {
        var menu = await _menuRepository.GetByIdAsync(id);

        var requestor = await _publisher.Send(new GetIdentityById(requestorId));

        if (!requestor.Role.IsRestaurantEmployee())
            return Result.Success(_mapper.Map<MenuInfoDto>(menu));

        return menu.HasContact(requestorId) ? Result.Success(_mapper.Map<MenuInfoDto>(menu)) : Result.NotFound();
    }

    public async Task<Result> UpdateAsync(Guid id, UpdateMenuDto updateRequest)
    {
        if (await _validationProvider.ValidateModelAsync(updateRequest) is var valRes && !valRes.IsSuccess)
            return valRes;

        var menu = await _menuRepository.GetByIdAsync(id);
        if (menu == default)
            throw new KeyNotFoundException();

        menu.AddOrEditContact(updateRequest.PhoneNumber,
                              updateRequest.Email,
                              updateRequest.Address);
        menu.Edit(updateRequest.Name);

        await _menuRepository.UpdateAsync(menu);
        return Result.Success();
    }
}
