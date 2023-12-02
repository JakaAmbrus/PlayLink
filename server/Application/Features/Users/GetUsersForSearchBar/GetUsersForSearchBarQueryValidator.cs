using FluentValidation;

namespace Application.Features.Users.GetUsersForSearchBar
{
    public class GetUsersForSearchBarQueryValidator : AbstractValidator<GetUsersForSearchBarQuery>
    {
        public GetUsersForSearchBarQueryValidator()
        {
            RuleFor(x => x.AuthUserId)
                .NotEmpty().WithMessage("Authenticated user Id required.");
        }
    }
}
