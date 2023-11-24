using FluentValidation;
using FluentValidation.Results;

namespace Application.Features.Comments.UploadComment
{
    public class UploadCommentCommandValidator : AbstractValidator<UploadCommentCommand>
    {
        public UploadCommentCommandValidator()
        {
            RuleFor(x => x.CommentContent)
                .NotNull().WithMessage("Comment content is required.");

            RuleFor(x => x.CommentContent.PostId)
                .GreaterThan(0).WithMessage("Invalid Post Id");

            RuleFor(x => x.CommentContent.Content)
                .NotNull().WithMessage("Comment content is required.")
                .MinimumLength(1).WithMessage("Comment content must contain at least 1 character.")
                .MaximumLength(300).WithMessage("Comment content must not exceed 300 characters.");
        }

        protected override bool PreValidate(ValidationContext<UploadCommentCommand> context, ValidationResult result)
        {
            var command = context.InstanceToValidate;

            command.CommentContent.Content = command.CommentContent?.Content?.Trim();

            return true;
        }
    }
}
