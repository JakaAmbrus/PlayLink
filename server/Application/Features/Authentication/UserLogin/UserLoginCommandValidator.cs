using FluentValidation;

namespace Application.Features.Authentication.UserLogin
{
    public class UserLoginCommandValidator : AbstractValidator<UserLoginCommand>
    {
        public UserLoginCommandValidator()
        {
            RuleFor(x => x.Username)
                .NotEmpty().WithMessage("Username required.");

            RuleFor(x => x.Password)
                .NotEmpty().WithMessage("Password required.");
        }

        protected override bool PreValidate(ValidationContext<UserLoginCommand> context, FluentValidation.Results.ValidationResult result)
        {
            var command = context.InstanceToValidate;

            command.Username = command.Username?.Trim();

            return true;
        }
    }
}
