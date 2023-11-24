using FluentValidation;

namespace Application.Features.Likes.LikeComment
{
    public class LikeCommentCommandValidator : AbstractValidator<LikeCommentCommand>
    {
        public LikeCommentCommandValidator()
        {
            RuleFor(x => x.CommentId)
                .GreaterThan(0).WithMessage("Invalid Comment Id");
        }
    }
}
