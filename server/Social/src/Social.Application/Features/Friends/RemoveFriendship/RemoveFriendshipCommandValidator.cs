using FluentValidation;

namespace Social.Application.Features.Friends.RemoveFriendship
{
    public class RemoveFriendshipCommandValidator : AbstractValidator<RemoveFriendshipCommand>
    {
        public RemoveFriendshipCommandValidator()
        {
            RuleFor(x => x.ProfileUsername)
                .NotEmpty().WithMessage("Profile username is required.");
                

            RuleFor(x => x.AuthUserId)
                .NotEmpty().WithMessage("Auth user id is required.");
        }
    }
}
