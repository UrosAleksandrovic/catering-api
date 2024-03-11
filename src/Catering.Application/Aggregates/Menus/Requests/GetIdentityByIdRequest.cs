using Catering.Domain.Aggregates.Identity;
using MediatR;

namespace Catering.Application.Aggregates.Menus.Requests;

internal record GetIdentityById(string Id) : IRequest<Identity>;
