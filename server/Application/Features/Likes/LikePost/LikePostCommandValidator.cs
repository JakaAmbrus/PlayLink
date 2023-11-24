using FluentValidation;

namespace Application.Features.Likes.LikePost
{
    public class LikePostCommandValidator : AbstractValidator<LikePostCommand>
    {
        public LikePostCommandValidator() 
        {
            RuleFor(x => x.PostId)
                .GreaterThan(0).WithMessage("Invalid Post Id");
        }
    }
}
