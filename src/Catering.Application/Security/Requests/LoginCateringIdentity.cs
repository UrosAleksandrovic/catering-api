using Catering.Application.Results;
using MediatR;

namespace Catering.Application.Security.Requests;

public record LoginCateringIdentity(string Login, string Password) : IRequest<Result<string>>;
