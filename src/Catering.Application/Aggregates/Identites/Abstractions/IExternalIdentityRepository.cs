﻿using Catering.Domain.Entities.IdentityAggregate;

namespace Catering.Application.Aggregates.Identites.Abstractions;

public interface IExternalIdentityRepository : IBaseRepository<ExternalIdentity>, IIdentityRepository<ExternalIdentity>
{
}