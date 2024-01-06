using FluentValidation;

namespace Application.Features.Messages.MarkMessageAsRead
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
