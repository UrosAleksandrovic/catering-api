﻿using Catering.Domain.Entities.MenuAggregate;

namespace Catering.Application.Aggregates.Menus.Abstractions;

public interface IMenuRepository : IBaseCrudRepository<Menu>
{
    Task<Menu> GetByContactIdAsync(string contactId);
}
