using FluentValidation;
using FluentValidation.Results;

namespace Application.Features.Comments.UploadComment
{
    public class UploadCommentCommandValidator : AbstractValidator<UploadCommentCommand>
    {
        public UploadCommentCommandValidator()
        {

            RuleFor(x => x.PostId)
                .NotEmpty().WithMessage("Post Id required.");

            RuleFor(x => x.Content)
                .NotNull().WithMessage("Comment content is required.")
                .MaximumLength(300).WithMessage("Comment content must not exceed 300 characters.");

            RuleFor(x => x.AuthUserId)
                .NotEmpty().WithMessage("Authorized user Id required.");
        }

        protected override bool PreValidate(ValidationContext<UploadCommentCommand> context, ValidationResult result)
        {
            var command = context.InstanceToValidate;

            command.Content = command.Content?.Trim();

            return true;
        }
    }
}
