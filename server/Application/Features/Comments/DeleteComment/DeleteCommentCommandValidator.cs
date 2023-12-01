using Application.Utils;
using FluentValidation;

namespace Application.Features.Comments.DeleteComment
{
    public class DeleteCommentCommandValidator : AbstractValidator<DeleteCommentCommand>
    {
        public DeleteCommentCommandValidator()
        {
            RuleFor(x => x.CommentId)
                .NotEmpty().WithMessage("CommentId required.");

            RuleFor(x => x.AuthUserId)
                .NotEmpty().WithMessage("AuthUserId required.");

            RuleFor(x => x.AuthUserRoles)
                .Must(ValidationUtils.BeValidRole).WithMessage("Invalid role detected.");
        }
    }
}
