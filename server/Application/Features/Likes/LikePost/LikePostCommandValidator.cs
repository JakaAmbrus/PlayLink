using FluentValidation;

namespace Application.Features.Likes.LikePost
{
    public class LikePostCommandValidator : AbstractValidator<LikePostCommand>
    {
        public LikePostCommandValidator() 
        {
            RuleFor(x => x.PostId)
                .NotEmpty().WithMessage("Post Id required.");

            RuleFor(x => x.AuthUserId)
                .NotEmpty().WithMessage("Authenticated user Id required.");
        }
    }
}
