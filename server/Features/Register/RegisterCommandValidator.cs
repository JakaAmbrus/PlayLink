using FluentValidation;
using FluentValidation.Results;

namespace server.Features.Register
{
    public class RegisterCommandValidator : AbstractValidator<RegisterCommand>
    {
        public RegisterCommandValidator()
        {
            RuleFor(x => x.FullName)
                .NotEmpty()
                .Must(x => x.Length > 2 && x.Length < 30 && x.Contains(" "))
                .Matches(@"^[a-zA-Z\s]*$")
                .WithMessage("Fullname invalid");

        }
        protected override bool PreValidate(ValidationContext<RegisterCommand> context, ValidationResult result)
        {
            var command = context.InstanceToValidate;

            if (command == null) return false;

            command.FullName = command.FullName?.Trim();

            return true;
        }
    }
}
