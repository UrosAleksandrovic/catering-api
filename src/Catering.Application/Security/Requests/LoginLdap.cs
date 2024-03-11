using Catering.Application.Results;
using MediatR;

namespace Catering.Application.Security.Requests;

public record LoginLdap(string Login, string Password) : IRequest<Result<string>>;
