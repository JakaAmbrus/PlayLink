using FluentValidation;

namespace Application.Features.Likes.UnlikePost
{
    public class UnlikePostCommandValidator : AbstractValidator<UnlikePostCommand>
    {
        public UnlikePostCommandValidator()
        {
            RuleFor(x => x.PostId)
                .GreaterThan(0).WithMessage("Invalid Post Id"); ;
        }
    }
}
