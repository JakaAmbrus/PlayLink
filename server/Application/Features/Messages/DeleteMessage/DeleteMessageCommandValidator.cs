using FluentValidation;

namespace Application.Features.Messages.DeleteMessage
{
    public class DeleteMessageCommandValidator : AbstractValidator<DeleteMessageCommand>
    {
        public DeleteMessageCommandValidator()
        {
            RuleFor(x => x.PrivateMessageId)
                .GreaterThan(0).WithMessage("Invalid Private Messsage Id");
        }
    }
}
