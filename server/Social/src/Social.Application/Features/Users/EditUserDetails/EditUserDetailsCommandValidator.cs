using FluentValidation;
using Social.Application.Utils;

namespace Social.Application.Features.Users.EditUserDetails
{
    public class EditUserDetailsCommandValidator : AbstractValidator<EditUserDetailsCommand>
    {
        public EditUserDetailsCommandValidator()
        {
            RuleFor(x => x.EditUserDto.Username)
                .NotEmpty().WithMessage("Sending user required.");
                
            RuleFor(x => x.EditUserDto.Description)
                .MaximumLength(200).WithMessage("Description cannot be longer than 200 characters.");

            RuleFor(x => x.EditUserDto.Country)
                .Must(ValidationUtils.IsValidCountry).WithMessage("Invalid country")
                .When(x => !string.IsNullOrEmpty(x.EditUserDto.Country));

            RuleFor(x => x.EditUserDto.PhotoFile)
                .Must(file => ValidationUtils.IsAppropriateSizeFile(file, 5)).WithMessage("Photo must be smaller than 5MB.")
                .Must(ValidationUtils.IsAValidTypeFile).WithMessage("Photo must be a PNG or JPEG.");

            RuleFor(x => x.AuthUserId)
                .NotEmpty().WithMessage("Authenticated user Id required.");

            RuleFor(x => x.AuthUserRoles)
                .Must(ValidationUtils.IsValidRole).WithMessage("Invalid user role.");
        }

        protected override bool PreValidate(ValidationContext<EditUserDetailsCommand> context, FluentValidation.Results.ValidationResult result)
        {
            var command = context.InstanceToValidate;

            command.EditUserDto.Description = command.EditUserDto?.Description?.Trim();
            command.EditUserDto.Country = command.EditUserDto?.Country?.Trim();

            return true;
        }
    }
}
