using FluentValidation;

namespace Application.Features.Likes.GetCommentLikes
{
    public class GetCommentLikesQueryValidator : AbstractValidator<GetCommentLikesQuery>
    {
        public GetCommentLikesQueryValidator()
        {
            RuleFor(x => x.CommentId)
                .NotEmpty().WithMessage("Comment Id required.");
        }
    }
}
