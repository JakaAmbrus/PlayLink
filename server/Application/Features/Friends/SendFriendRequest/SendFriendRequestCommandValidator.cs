using FluentValidation;

namespace Application.Features.Friends.SendFriendRequest
{
    public class SendFriendRequestCommandValidator : AbstractValidator<SendFriendRequestCommand>
    {
        public SendFriendRequestCommandValidator()
        {
            RuleFor(x => x.ReceiverUsername)
                .NotEmpty().WithMessage("Username of the receiver required.");

            RuleFor(x => x.AuthUserId)
                .NotEmpty().WithMessage("AuthUserId required.");
        }
    }
}
