using FluentValidation;

namespace Social.Application.Features.Likes.GetCommentLikes
{
    public class GetCommentLikesQueryValidator : AbstractValidator<GetCommentLikesQuery>
    {
        public GetCommentLikesQueryValidator()
        {
            RuleFor(x => x.CommentId)
                .NotEmpty().WithMessage("Comment Id required.");

            RuleFor(x => x.AuthUserId)
                .NotEmpty().WithMessage("Authorized User Id required.");
        }
    }
}
