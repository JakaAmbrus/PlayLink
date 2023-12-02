using Application.Utils;
using FluentValidation;

namespace Application.Features.Posts.DeletePost
{
    public class DeletePostCommandValidator : AbstractValidator<DeletePostCommand>
    {
        public DeletePostCommandValidator()
        {
            RuleFor(x => x.PostId)
                .NotEmpty().WithMessage("PostId required.");

            RuleFor(x => x.AuthUserId)
              .NotEmpty().WithMessage("Authenticated user Id required.");

            RuleFor(x => x.AuthUserRoles)
                .Must(ValidationUtils.BeValidRole).WithMessage("Invalid user role.");
        }
    }
}
