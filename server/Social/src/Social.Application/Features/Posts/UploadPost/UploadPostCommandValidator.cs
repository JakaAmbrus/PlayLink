using FluentValidation;
using FluentValidation.Results;
using Social.Application.Utils;

namespace Social.Application.Features.Posts.UploadPost
{
    public class UploadPostCommandValidator : AbstractValidator<UploadPostCommand>
    {
        public UploadPostCommandValidator()
        {
            RuleFor(x => x.PostContentDto)
                .NotNull().WithMessage("Post content is required");

            RuleFor(x => x.PostContentDto.Description)
                .MaximumLength(400).WithMessage("Description must not exceed 400 characters of space");

            RuleFor(x => x.PostContentDto.PhotoFile)
                .Must(file => ValidationUtils.IsAppropriateSizeFile(file, 5)).WithMessage("Photo must be smaller than 5MB")
                .Must(ValidationUtils.IsAValidTypeFile).WithMessage("Photo must be a PNG or JPEG");

            RuleFor(x => x.AuthUserId)
                .NotEmpty().WithMessage("Authenticated user Id required.");
        }

        protected override bool PreValidate(ValidationContext<UploadPostCommand> context, ValidationResult result)
        {
            var command = context.InstanceToValidate;

            command.PostContentDto.Description = command.PostContentDto?.Description?.Trim();
            
            return true;
        }
    }
}
