using FluentValidation;
using FluentValidation.Results;
using Shared.Core.Security;

namespace Shield.Api.Features.SignInGuest;

public class SignInGuestCommandValidator : AbstractValidator<SignInGuestCommand>
{
    public SignInGuestCommandValidator()
    {
        RuleFor(x => x.Role)
            .Must(role => role == Roles.Member || role == Roles.Moderator || role == Roles.Admin)
            .When(x => !string.IsNullOrEmpty(x.Role))
            .WithMessage("Not a valid Guest role.");
    }
    
    protected override bool PreValidate(ValidationContext<SignInGuestCommand> context, ValidationResult result)
    {
        if (string.IsNullOrEmpty(context.InstanceToValidate.Role))
        {
            context.InstanceToValidate.Role = Roles.Member;
        }
            
        return true;
    }
}