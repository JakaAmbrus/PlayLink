using FluentValidation;

namespace Application.Features.Likes.UnlikePost
{
    public class UnlikePostCommandValidator : AbstractValidator<UnlikePostCommand>
    {
        public UnlikePostCommandValidator()
        {
            RuleFor(x => x.PostId)
                .NotEmpty().WithMessage("Post Id required.");

            RuleFor(x => x.AuthUserId)
                .NotEmpty().WithMessage("Authenticated user Id required.");
        }
    }
}
