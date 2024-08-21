using Social.Application.Utils;
using FluentValidation;

namespace Social.Application.Features.Admin.AdminEditRoles
{
    public class AdminEditRolesCommandValidator : AbstractValidator<AdminEditRolesCommand>
    { 
        public AdminEditRolesCommandValidator()
        {
            RuleFor(x => x.AppUserId)
                .NotEmpty().WithMessage("App user Id required.");

            RuleFor(x => x.AuthUserId)
                .NotEmpty().WithMessage("Authenticated user Id required.");
        }
    }
}
