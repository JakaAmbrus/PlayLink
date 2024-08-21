using FluentValidation;

namespace Social.Application.Features.Friends.RespondToFriendRequest
{
    public class RespondToFriendRequestCommandValidator : AbstractValidator<RespondToFriendRequestCommand>
    {
        public RespondToFriendRequestCommandValidator()
        {
            RuleFor(x => x.FriendRequestResponse.FriendRequestId)
                .NotEmpty().WithMessage("Friend request Id required.");

            RuleFor(x => x.FriendRequestResponse.Accept)
                .NotNull().WithMessage("Friend request response required.");

            RuleFor(x => x.AuthUserId)
                .NotEmpty().WithMessage("Auth user Id required.");
        }
    }
}
