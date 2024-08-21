using FluentValidation;

namespace Social.Application.Features.Friends.RemoveFriendRequest
{
    public class RemoveFriendRequestCommandValidator : AbstractValidator<RemoveFriendRequestCommand>
    {
        public RemoveFriendRequestCommandValidator()
        {
            RuleFor(x => x.AuthUserId)
                .NotEmpty().WithMessage("Auth user Id required.");

            RuleFor(x => x.FriendRequestId)
                .NotEmpty().WithMessage("Friend request Id required.");
        }
    }
}
