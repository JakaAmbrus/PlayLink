using FluentValidation;
using FluentValidation.Results;

namespace WebAPI.Features.Register
{
    public class RegisterCommandValidator : AbstractValidator<RegisterCommand>
    {
        public RegisterCommandValidator()
        {
            RuleFor(x => x.Username)
                .NotEmpty()
                .Must(x => x.Length > 2 && x.Length < 30)
                .Matches(@"^[a-zA-Z0-9_.-]*$")
                .WithMessage("Username invalid");

            RuleFor(x => x.Password)
                .NotEmpty()
                .Must(x => x.Length > 2 && x.Length < 20)
                .WithMessage("Password invalid");

            RuleFor(x => x.FullName)
                .NotEmpty()
                .Must(x => x.Length > 2 && x.Length < 30 && x.Contains(" "))
                .Matches(@"^[a-zA-Z\s]*$")
                .WithMessage("Fullname invalid");
                
            RuleFor(x => x.Country)
                .NotEmpty()
                .Matches(@"^[a-zA-Z\s]*$")
                .Must(x => x.Length > 2 && x.Length < 20)
                .WithMessage("Country invalid");

            RuleFor(x => x.City)
                .NotEmpty()
                .Matches(@"^[a-zA-Z\s]*$")
                .Must(x => x.Length > 2 && x.Length < 20)
                .WithMessage("City invalid");

            RuleFor(x => x.DateOfBirth)
                .NotEmpty()
                .Must(x => x.Year < DateTime.UtcNow.Year - 12)
                .WithMessage("You must be at least 12 years old to register");                                
        }
        protected override bool PreValidate(ValidationContext<RegisterCommand> context, ValidationResult result)
        {
            var command = context.InstanceToValidate;

            if (command == null)
            {
                return false;
            }

            command.FullName = command.FullName?.Trim();
            command.Username = command.Username?.Trim();
            command.Password = command.Password?.Trim();
            command.Country = command.Country?.Trim();
            command.City = command.City?.Trim();

            return true;
        }
    }
}
