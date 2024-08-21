using FluentValidation;

namespace Social.Application.Features.Users.GetUsersUniqueCountries
{
    public class GetUsersUniqueCountriesQueryValidator : AbstractValidator<GetUsersUniqueCountriesQuery>
    {
        public GetUsersUniqueCountriesQueryValidator()
        {
            RuleFor(x => x.AuthUserId)
                .NotEmpty().WithMessage("Authenticated user Id required.");
        }
    }
}
