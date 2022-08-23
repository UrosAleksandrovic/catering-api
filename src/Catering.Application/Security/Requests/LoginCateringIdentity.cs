using MediatR;

namespace Catering.Application.Security.Requests;

public class LoginCateringIdentity : IRequest<string>
{
    public string Login { get; set; }
    public string Password { get; set; }
}