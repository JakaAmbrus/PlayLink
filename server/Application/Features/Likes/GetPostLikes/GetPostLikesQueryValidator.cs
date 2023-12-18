using FluentValidation;

namespace Application.Features.Likes.GetPostLikes
{
    public class GetPostLikesQueryValidator : AbstractValidator<GetPostLikesQuery>
    {
        public GetPostLikesQueryValidator()
        {
            RuleFor(x => x.PostId)
                .NotEmpty().WithMessage("Post Id required.");
        }
    }
}
