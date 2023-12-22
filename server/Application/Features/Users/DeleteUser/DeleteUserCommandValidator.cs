using FluentValidation;

namespace Application.Features.Users.DeleteUser
{
    public class DeleteUserCommandValidator : AbstractValidator<DeleteUserCommand>
    {
        public DeleteUserCommandValidator()
        {
            RuleFor(x => x.AuthUserId)
                .NotEmpty().WithMessage("AuthUserId required.");

            RuleFor(x => x.AuthUserRoles)
                .NotEmpty().WithMessage("AuthUserRoles required.");
        }
    }
}
