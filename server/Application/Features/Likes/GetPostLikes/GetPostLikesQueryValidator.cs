using FluentValidation;

namespace Application.Features.Likes.GetPostLikes
{
    public class GetPostLikesQueryValidator : AbstractValidator<GetPostLikesQuery>
    {
        public GetPostLikesQueryValidator()
        {
            RuleFor(x => x.PostId)
                .NotEmpty().WithMessage("Post Id required.");

            RuleFor(x => x.AuthUserId)
                .NotEmpty().WithMessage("Authorized User Id required.");
        }
    }
}
