using MediatR;

namespace Catering.Application.Security.Requests;

public class LoginLdap : IRequest<string>
{
    public string Login { get; set; }
    public string Password { get; set; }
}
