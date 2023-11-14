using FluentValidation;
using FluentValidation.Results;

namespace Application.Features.Posts.UploadPost
{
    public class UploadPostCommandValidator : AbstractValidator<UploadPostCommand>
    {
        public UploadPostCommandValidator()
        {
            RuleFor(x => x.PostContentDto.Description)
                .NotEmpty().WithMessage("Description is required.")
                .MaximumLength(300).WithMessage("Description cannot exceed 300 characters.");

            RuleFor(x => x.PostContentDto.PhotoUrl)
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

            command.PostContentDto.Description = command.PostContentDto?.Description?.Trim();
            
            return true;
        }
    }
}
