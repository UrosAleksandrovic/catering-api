using FluentValidation;

namespace Catering.Application.Aggregates.Identities.Dtos.Validators;

public class CreateIdentityInvitationDtoValidator : AbstractValidator<CreateIdentityInvitationDto>
{
    public CreateIdentityInvitationDtoValidator()
    {
        RuleFor(e => e.Email).NotEmpty().EmailAddress();
        RuleFor(e => e.FirstName).NotEmpty().MinimumLength(2);
    }
}
