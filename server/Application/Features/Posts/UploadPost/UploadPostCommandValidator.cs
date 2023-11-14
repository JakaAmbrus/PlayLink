using FluentValidation;
using FluentValidation.Results;
using Microsoft.AspNetCore.Http;

namespace Application.Features.Posts.UploadPost
{
    public class UploadPostCommandValidator : AbstractValidator<UploadPostCommand>
    {
        public UploadPostCommandValidator()
        {
            RuleFor(x => x.PostContentDto.Description)
                .NotEmpty().WithMessage("Description is required.")
                .MaximumLength(300).WithMessage("Description cannot exceed 300 characters.");

            RuleFor(x => x.PostContentDto.PhotoFile)
                .Must(BeAppropriateSize).WithMessage("Photo must be smaller than 4MB.")
                .Must(BeAValidType).WithMessage("Photo must be a PNG or JPEG.");
        }
        /*        private bool IsValidUrl(string photoUrl)
                {
                    return Uri.TryCreate(photoUrl, UriKind.Absolute, out var uriResult)
                        && (uriResult.Scheme == Uri.UriSchemeHttp 
                        || uriResult.Scheme == Uri.UriSchemeHttps);
                }*/
        private bool BeAppropriateSize(IFormFile file)
        {
            // The maximum size is 4 MB
            return file == null || file.Length <= 4 * 1024 * 1024;
        }
        private bool BeAValidType(IFormFile file)
        {
            //Allow only JPEG and PNG files
            var allowedTypes = new[] { "image/jpeg", "image/png" };
            return file == null || allowedTypes.Contains(file.ContentType);
        }
        protected override bool PreValidate(ValidationContext<UploadPostCommand> context, ValidationResult result)
        {
            var command = context.InstanceToValidate;

            command.PostContentDto.Description = command.PostContentDto?.Description?.Trim();
            
            return true;
        }
    }
}
