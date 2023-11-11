using FluentValidation;
using FluentValidation.Results;

namespace Application.Features.Authentication.UserRegistration
{
    public class UserRegistrationCommandValidator : AbstractValidator<UserRegistrationCommand>
    {
        public UserRegistrationCommandValidator()
        {
            RuleFor(x => x.Username)
                .NotEmpty().WithMessage("Username required.")
                .MinimumLength(4).WithMessage("Username must contain at least 4 characters.")
                .MaximumLength(20).WithMessage("Username cannot exceed 20 characters.")
                .Matches(@"^[a-zA-Z\s]*$").WithMessage("Username must include standard characters");

            RuleFor(x => x.Password)
                .NotEmpty().WithMessage("Password required.")
                .MinimumLength(4).WithMessage("Password must contain at least 4 characters.")
                .MaximumLength(20).WithMessage("Password cannot exceed 20 characters.")
                .Matches(@".*\d.*").WithMessage("Password must contain at least one number.");

            RuleFor(x => x.FullName)
                .NotEmpty().WithMessage("Full Name required.")
                .Must(x => x.Length > 4 && x.Length < 30 && x.Contains(" ")).WithMessage("Full Name must be between 4 and 30 characters and include both first and last name.")
                .Matches(@"^[a-zA-Z\s]*$").WithMessage("Full name must include standard characters")
                .When(x => x.FullName != null);

            RuleFor(x => x.Country)
                .NotEmpty().WithMessage("Country required.")
                .Matches(@"^[a-zA-Z\s]*$").WithMessage("Country must include standard characters")
                .MinimumLength(4).WithMessage("No Country contains less than 4 characters, abstain from abbreviations.")
                .MaximumLength(30).WithMessage("Country cannot exceed 30 characters.");

            RuleFor(x => x.City)
                .NotEmpty().WithMessage("City required.")
                .Matches(@"^[a-zA-Z\s]*$").WithMessage("City must include standard characters.")
                .MinimumLength(2).WithMessage("City must contain at least 4 characters.")
                .MaximumLength(30).WithMessage("Country cannot exceed 30 characters.");

            RuleFor(x => x.DateOfBirth)
                .NotEmpty().WithMessage("Date of Birth required.")
                .Must(x => x.Year < DateTime.UtcNow.Year - 12).WithMessage("You must be at least 12 years old to register");
        }
        protected override bool PreValidate(ValidationContext<UserRegistrationCommand> context, ValidationResult result)
        {
            var command = context.InstanceToValidate;

            if (command == null)
            {
                return false;
            }

            return true;
        }
    }
}
