using FluentValidation;

namespace Application.Features.Users.GetUsers
{
    public class GetUsersQueryValidator : AbstractValidator<GetUsersQuery>
    {
        public GetUsersQueryValidator()
        {
            RuleFor(x => x.Params)
                .NotEmpty().WithMessage("Params required.");

            RuleFor(x => x.AuthUserId)
                .NotEmpty().WithMessage("Authenticated user Id required.");
        }
    }
}
