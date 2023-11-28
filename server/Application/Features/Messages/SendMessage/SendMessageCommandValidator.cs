using FluentValidation;

namespace Application.Features.Messages.SendMessage
{
    public class SendMessageCommandValidator : AbstractValidator<SendMessageCommand>
    {
        public SendMessageCommandValidator()
        {
            RuleFor(x => x.CreateMessageDto.Content)
                .NotEmpty().WithMessage("Message cannot be empty")
                .MaximumLength(100).WithMessage("Message cannot exceed 100 characters");

            RuleFor(x => x.CreateMessageDto.RecipientUsername)
                .NotEmpty().WithMessage("Must be valid recipient");
        }
    }
}
