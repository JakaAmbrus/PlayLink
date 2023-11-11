using FluentValidation;
using FluentValidation.Results;

namespace Application.Features.Posts.UploadPost
{
    public class UploadPostCommandValidator : AbstractValidator<UploadPostCommand>
    {
        public UploadPostCommandValidator()
        {
            RuleFor(x => x.PostDto.Description)
                .NotEmpty().WithMessage("Description is required.")
                .MaximumLength(300).WithMessage("Description cannot exceed 300 characters.");

            RuleFor(x => x.PostDto.PhotoUrl)
                .Must(IsValidUrl).WithMessage("Photo URL is not valid.");
        }
        private bool IsValidUrl(string photoUrl)
        {
            return Uri.TryCreate(photoUrl, UriKind.Absolute, out var uriResult)
                && (uriResult.Scheme == Uri.UriSchemeHttp 
                || uriResult.Scheme == Uri.UriSchemeHttps);
        }
        protected override bool PreValidate(ValidationContext<UploadPostCommand> context, ValidationResult result)
        {
            var command = context.InstanceToValidate;

            if (command == null)
            {
                result.Errors.Add(new ValidationFailure("Command", "Command cannot be null."));
                return false;
            }
            return true;
        }
    }
}
