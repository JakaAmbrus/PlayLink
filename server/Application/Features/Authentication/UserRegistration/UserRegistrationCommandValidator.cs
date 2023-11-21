using Application.Utils;
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
                .MaximumLength(10).WithMessage("Username cannot exceed 10 characters.")
                .Matches(@"^[a-zA-Z\s]*$").WithMessage("Username must include standard characters");

            RuleFor(x => x.Password)
                .NotEmpty().WithMessage("Password required.")
                .MinimumLength(4).WithMessage("Password must contain at least 4 characters.")
                .MaximumLength(20).WithMessage("Password cannot exceed 20 characters.")
                .Matches(@".*\d.*").WithMessage("Password must contain at least one number.");

            RuleFor(x => x.Gender)
                .NotEmpty().WithMessage("Gender required.")
                .Must(MaleOrFemaleGender).WithMessage("Gender must be either 'Male' or 'Female', this is for certain features, do not wish to offend")
                .When(x => x.Gender != null);

            RuleFor(x => x.FullName)
                .NotEmpty().WithMessage("Full Name required.")
                .Must(x => x.Length > 4 && x.Length < 30 && x.Contains(" ")).WithMessage("Full Name must be between 4 and 30 characters and include both first and last name.")
                .Matches(@"^[a-zA-Z\s]*$").WithMessage("Full name must include standard characters")
                .Must(CheckFirstNameLenght).WithMessage("First name cannot exceed 12 characters and must have second name.")
                .When(x => x.FullName != null);

            RuleFor(x => x.Country)
                .NotEmpty().WithMessage("Country required.")
                .Must(ValidationUtils.IsValidCountry).WithMessage("Invalid country");

            RuleFor(x => x.DateOfBirth)
                .NotEmpty().WithMessage("Date of Birth required.")
                .Must(x => x.Year < DateTime.UtcNow.Year - 12).WithMessage("You must be at least 12 years old to register")
                .Must(x => x.Year > DateTime.UtcNow.Year - 99).WithMessage("Your age must be realistic");
        }
        private bool MaleOrFemaleGender(string gender)
        {
            return gender.Equals("male", StringComparison.OrdinalIgnoreCase) ||
                   gender.Equals("female", StringComparison.OrdinalIgnoreCase);
        }

        private bool CheckFirstNameLenght(string fullName)
        {
            var words = fullName.Split(' ');

            if (words.Length == 0)
            {
                return false;
            }

            string firstName = words[0];
            string secondName = words[1];

            return firstName.Length <= 12 && secondName.Length > 0;
        }

        protected override bool PreValidate(ValidationContext<UserRegistrationCommand> context, ValidationResult result)
        {
            var command = context.InstanceToValidate;

            command.Username = command.Username?.ToLower().Trim();
            command.Password = command.Password?.Trim();
            command.Gender = command.Gender?.ToLower().Trim();
            command.FullName = command.FullName?.Trim();
            command.Country = command.Country?.Trim();

            return true;
        }
    }
}
