using FluentValidation;

namespace Social.Application.Features.MessageGroups.GetConnection
{
    public class GetConnectionQueryValidator : AbstractValidator<GetConnectionQuery>
    {
        public GetConnectionQueryValidator()
        {
            RuleFor(x => x.ConnectionId)
                .NotEmpty().WithMessage("Connection Id required.");
        }
    }
}
