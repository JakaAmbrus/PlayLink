using FluentValidation;

namespace Application.Features.Admin.AdminGetUsers
{
    public class AdminGetUsersQueryValidator : AbstractValidator<AdminGetUsersQuery>
    {
        public AdminGetUsersQueryValidator()
        {
            RuleFor(x => x.Params)
                .NotNull().WithMessage("Params required.");

            RuleFor(x => x.Params.PageNumber)
                .GreaterThan(0).WithMessage("Page Number must be greater than 0.");

            RuleFor(x => x.Params.PageSize)
                .GreaterThan(0).WithMessage("Page Size must be greater than 0.");

            RuleFor(x => x.AuthUserId)
                .NotEmpty().WithMessage("Authenticated user Id required.");
        }
    }
}
