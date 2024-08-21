using FluentValidation;
using FluentValidation.Results;

namespace Social.Application.Features.Comments.UploadComment
{
    public class UploadCommentCommandValidator : AbstractValidator<UploadCommentCommand>
    {
        public UploadCommentCommandValidator()
        {

            RuleFor(x => x.Comment.PostId)
                .NotEmpty().WithMessage("Post Id required.");

            RuleFor(x => x.Comment.Content)
                .NotNull().WithMessage("Comment content is required.")
                .MaximumLength(300).WithMessage("Comment content must not exceed 300 characters.");

            RuleFor(x => x.AuthUserId)
                .NotEmpty().WithMessage("Authorized user Id required.");
        }

        protected override bool PreValidate(ValidationContext<UploadCommentCommand> context, ValidationResult result)
        {
            var command = context.InstanceToValidate;

            command.Comment.Content = command.Comment.Content?.Trim();

            return true;
        }
    }
}
