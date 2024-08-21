using FluentValidation;

namespace Social.Application.Features.Likes.UnlikeComment
{
    public class UnlikeCommentCommandValidator : AbstractValidator<UnlikeCommentCommand>
    { 
        public UnlikeCommentCommandValidator()
        {
            RuleFor(x => x.CommentId)
                 .NotEmpty().WithMessage("CommentId required.");

            RuleFor(x => x.AuthUserId)
                .NotEmpty().WithMessage("Authenticated user Id required.");
        }
    }
}
