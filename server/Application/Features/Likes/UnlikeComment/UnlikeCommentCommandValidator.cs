using FluentValidation;

namespace Application.Features.Likes.UnlikeComment
{
    public class UnlikeCommentCommandValidator : AbstractValidator<UnlikeCommentCommand>
    { 
        public UnlikeCommentCommandValidator()
        {
            RuleFor(x => x.CommentId)
                .GreaterThan(0).WithMessage("Invalid Comment Id"); ;
        }
    }
}
