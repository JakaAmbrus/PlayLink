using Application.Utils;
using FluentValidation;

namespace Application.Features.Admin.AdminUserDelete
{
    public class AdminUserDeleteCommandValidator : AbstractValidator<AdminUserDeleteCommand>
    {
        public AdminUserDeleteCommandValidator()
        {
            RuleFor(x => x.AppUserId)
                .NotEmpty().WithMessage("User Id required.");

            RuleFor(x => x.AuthUserId)
                .NotEmpty().WithMessage("Authenticated user Id required.");

            RuleFor(x => x.AuthUserRoles)
                .Must(ValidationUtils.IsValidRole).WithMessage("Invalid user role.");
        }
    }
}
