using Catering.Application.Aggregates.Items.Abstractions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Catering.Api.Controllers;

[Route("/api/items")]
public class ItemsController : ControllerBase
{
    private readonly IItemManagementAppService _itemsAppService;

    public ItemsController(IItemManagementAppService itemsAppService)
    {
        _itemsAppService = itemsAppService;
    }


}
