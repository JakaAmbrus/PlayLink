using FluentValidation;

namespace Social.Application.Features.Messages.GetMessageThread
{
    public class GetMessageThreadQueryValidator : AbstractValidator<GetMessageThreadQuery>
    {
        public GetMessageThreadQueryValidator()
        {
            RuleFor(x => x.ProfileUsername)
                .NotEmpty().WithMessage("Profile username is required");
            RuleFor(x => x.AuthUserId)
                .NotEmpty().WithMessage("Authenticated user Id required.");
        }
    }
}
