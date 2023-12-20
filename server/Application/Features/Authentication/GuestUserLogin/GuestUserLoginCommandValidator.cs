using FluentValidation;

namespace Application.Features.Authentication.GuestUserLogin
{
    public class GuestUserLoginCommandValidator : AbstractValidator<GuestUserLoginCommand>
    {
        public GuestUserLoginCommandValidator()
        {
            RuleFor(x => x.Role)
                .NotEmpty().WithMessage("Role required.");           
        }
    }
}
