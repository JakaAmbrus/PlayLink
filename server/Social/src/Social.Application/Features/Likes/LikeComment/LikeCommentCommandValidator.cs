using FluentValidation;

namespace Social.Application.Features.Likes.LikeComment
{
    public class LikeCommentCommandValidator : AbstractValidator<LikeCommentCommand>
    {
        public LikeCommentCommandValidator()
        {
            RuleFor(x => x.CommentId)
                .NotEmpty().WithMessage("CommentId required.");

            RuleFor(x => x.AuthUserId)
                .NotEmpty().WithMessage("Authenticated user Id required.");
        }
    }
}
