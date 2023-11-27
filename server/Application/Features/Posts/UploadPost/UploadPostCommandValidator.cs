using Application.Utils;
using FluentValidation;
using FluentValidation.Results;

namespace Application.Features.Posts.UploadPost
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
                .Must(ValidationUtils.BeAppropriateSize).WithMessage("Photo must be smaller than 4MB")
                .Must(ValidationUtils.BeAValidType).WithMessage("Photo must be a PNG or JPEG");
        }

        protected override bool PreValidate(ValidationContext<UploadPostCommand> context, ValidationResult result)
        {
            var command = context.InstanceToValidate;

            command.PostContentDto.Description = command.PostContentDto?.Description?.Trim();
            
            return true;
        }
    }
}
