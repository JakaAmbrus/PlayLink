using FluentValidation;

namespace Application.Features.Messages.GetMessageThread
{
    public class GetMessageThreadQueryValidator : AbstractValidator<GetMessageThreadQuery>
    {
        public GetMessageThreadQueryValidator()
        {
            RuleFor(x => x.RecipientUsername)
                .NotEmpty()
                .WithMessage("Recipient username is required");
        }
    }
}
