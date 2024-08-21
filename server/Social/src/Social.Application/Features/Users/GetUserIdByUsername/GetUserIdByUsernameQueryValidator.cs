using FluentValidation;

namespace Social.Application.Features.Users.GetUserIdFromUsername
{
    public class GetUserIdByUsernameQueryValidator : AbstractValidator<GetUserIdByUsernameQuery>
    {
        public GetUserIdByUsernameQueryValidator()
        {
            RuleFor(x => x.Username)
                .NotEmpty().WithMessage("Username required.");
        }
    }
}
