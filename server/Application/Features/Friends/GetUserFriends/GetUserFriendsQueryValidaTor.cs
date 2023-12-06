using FluentValidation;

namespace Application.Features.Friends.GetUserFriends
{
    public class GetUserFriendsQueryValidaTor : AbstractValidator<GetUserFriendsQuery>
    {
        public GetUserFriendsQueryValidaTor()
        {
            RuleFor(x => x.AuthUserId)
                .NotEmpty().WithMessage("Auth user Id required.");
        }
    }
}
