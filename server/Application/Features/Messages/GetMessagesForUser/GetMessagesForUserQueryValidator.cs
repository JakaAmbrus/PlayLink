using FluentValidation;

namespace Application.Features.Messages.GetMessagesForUser
{
    public class GetMessagesForUserQueryValidator : AbstractValidator<GetMessagesForUserQuery>
    {
        public GetMessagesForUserQueryValidator()
        {
            RuleFor(x => x.Params)
                .NotEmpty().WithMessage("Parameters required");

            RuleFor(x => x.AuthUserId)
                .NotEmpty().WithMessage("Authenticated user Id required.");
        }
    }
}
