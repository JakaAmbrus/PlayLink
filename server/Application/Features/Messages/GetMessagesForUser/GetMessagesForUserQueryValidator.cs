using FluentValidation;

namespace Application.Features.Messages.GetMessagesForUser
{
    public class GetMessagesForUserQueryValidator : AbstractValidator<GetMessagesForUserQuery>
    {
        public GetMessagesForUserQueryValidator()
        {
            RuleFor(x => x.Params)
                .NotEmpty().WithMessage("Parameters required");

            RuleFor(x => x.Params.PageNumber)
                .GreaterThan(0).WithMessage("Page number must be greater than 0.");

            RuleFor(x => x.Params.PageSize)
                .GreaterThan(0).WithMessage("Page size must be greater than 0.");

            RuleFor(x => x.AuthUserId)
                .NotEmpty().WithMessage("Authenticated user Id required.");
        }
    }
}
