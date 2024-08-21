using FluentValidation;

namespace Social.Application.Features.Friends.GetRelationshipStatus
{
    public class GetFriendshipStatusQueryValidator : AbstractValidator<GetFriendshipStatusQuery>
    {
        public GetFriendshipStatusQueryValidator()
        {
            RuleFor(x => x.ProfileUsername)
                .NotEmpty().WithMessage("Profile username is required");

            RuleFor(x => x.AuthUserId)
                .NotEmpty().WithMessage("Auth user Id required.");
        }
    }
}
