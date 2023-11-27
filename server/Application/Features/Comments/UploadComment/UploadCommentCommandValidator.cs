using FluentValidation;
using FluentValidation.Results;

namespace Application.Features.Comments.UploadComment
{
    public class UploadCommentCommandValidator : AbstractValidator<UploadCommentCommand>
    {
        public UploadCommentCommandValidator()
        {

            RuleFor(x => x.PostId)
                .GreaterThan(0).WithMessage("Invalid Post Id");

            RuleFor(x => x.CommentContentDto.Content)
                .NotNull().WithMessage("Comment content is required.")
                .MaximumLength(300).WithMessage("Comment content must not exceed 300 characters.");
        }

        protected override bool PreValidate(ValidationContext<UploadCommentCommand> context, ValidationResult result)
        {
            var command = context.InstanceToValidate;

            command.CommentContentDto.Content = command.CommentContentDto.Content?.Trim();

            return true;
        }
    }
}
