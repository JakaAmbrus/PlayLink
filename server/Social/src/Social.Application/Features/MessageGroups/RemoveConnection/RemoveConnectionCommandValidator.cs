using FluentValidation;

namespace Social.Application.Features.MessageGroups.RemoveConnection
{
    public class RemoveConnectionCommandValidator : AbstractValidator<RemoveConnectionCommand>
    {
        public RemoveConnectionCommandValidator()
        {
            RuleFor(x => x.ConnectionId)
                .NotEmpty().WithMessage("Connection Id required.");
        }
    }
}
