using FluentValidation;

namespace Social.Application.Features.Messages.GetMessageById
{
    public class GetMessageByIdQueryValidator : AbstractValidator<GetMessageByIdQuery>
    {
        public GetMessageByIdQueryValidator()
        {
            RuleFor(x => x.MessageId)
                .NotEmpty().WithMessage("Message Id required.");
        }
    }
}
