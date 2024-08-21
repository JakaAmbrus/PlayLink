using FluentValidation;

namespace Social.Application.Features.Users.GetUsers
{
    public class GetUsersQueryValidator : AbstractValidator<GetUsersQuery>
    {
        public GetUsersQueryValidator()
        {
            RuleFor(x => x.Params)
                .NotEmpty().WithMessage("Params required.");

            RuleFor(x => x.Params.PageNumber)
                .GreaterThan(0).WithMessage("Page Number must be greater than 0.");

            RuleFor(x => x.Params.PageSize)
                .GreaterThan(0).WithMessage("Page Size must be greater than 0.");

            RuleFor(x => x.AuthUserId)
                .NotEmpty().WithMessage("Authenticated user Id required.");
        }
    }
}
