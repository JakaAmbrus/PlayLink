using FluentValidation;

namespace Application.Features.MessageGroups.MarkMessageAsRead
{
    public class MarkMessageAsReadCommandValidator : AbstractValidator<MarkMessageAsReadCommand>
    {
        public MarkMessageAsReadCommandValidator()
        {
            RuleFor(x => x.MessageId)
                .NotEmpty().WithMessage("Connection Id required.");
        }
    }
}
