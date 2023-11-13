using FluentValidation;

namespace Application.Features.Posts.DeletePost
{
    public class DeletePostCommandValidator : AbstractValidator<DeletePostCommand>
    {
        public DeletePostCommandValidator()
        {
            RuleFor(x => x.PostId)
                .NotEmpty().WithMessage("PostId required.");
        }
    }
}
