using FluentValidation;

namespace Application.Features.Admin.AdminGetUsers
{
    public class AdminGetUsersQueryValidator : AbstractValidator<AdminGetUsersQuery>
    {
        public AdminGetUsersQueryValidator()
        {
            RuleFor(x => x.Params)
                .NotNull().WithMessage("Params required.");

            RuleFor(x => x.AuthUserId)
                .NotEmpty().WithMessage("Authenticated user Id required.");
        }
    }
}
