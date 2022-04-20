using Catering.Application.Security.Requests;
using Catering.Domain.Entities.IdentityAggregate;
using MediatR;

namespace Catering.Application.Security.Handlers;

public class LoginCustomerHandler : IRequestHandler<LoginCustomer, string>
{
    private readonly ITokenAtuhenticator<Customer> _customerAuthenticator;

    public LoginCustomerHandler(ITokenAtuhenticator<Customer> customerAuthenticator)
    {
        _customerAuthenticator = customerAuthenticator;
    }

    public Task<string> Handle(LoginCustomer request, CancellationToken cancellationToken)
    {
        return _customerAuthenticator.GenerateTokenAsync(request.Login, request.Password);
    }
}
