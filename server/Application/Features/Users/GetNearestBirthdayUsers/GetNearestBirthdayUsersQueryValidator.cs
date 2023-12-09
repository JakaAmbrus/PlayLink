using FluentValidation;

namespace Application.Features.Users.GetNearestBirthdayUsers
{
    public class GetNearestBirthdayUsersQueryValidator : AbstractValidator<GetNearestBirthdayUsersQuery>
    {
        public GetNearestBirthdayUsersQueryValidator()
        {
            RuleFor(x => x.AuthUserId)
                .NotEmpty().WithMessage("Authorized user Id required.");
        }
    }
}
