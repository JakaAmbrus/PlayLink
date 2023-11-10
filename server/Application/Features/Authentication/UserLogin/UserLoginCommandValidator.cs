using FluentValidation;
using FluentValidation.Results;

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

        protected override bool PreValidate(ValidationContext<UserLoginCommand> context, ValidationResult result)
        {
            var command = context.InstanceToValidate;

            if (command == null)
            {
                return false;
            }

            command.Username = command.Username.Trim();

            return true;
        }
    }
}
