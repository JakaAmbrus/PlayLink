using FluentValidation;

namespace Social.Application.Features.MessageGroups.GetMessageGroup
{
    public class GetMessageGroupQueryValidator : AbstractValidator<GetMessageGroupQuery>
    {
        public GetMessageGroupQueryValidator()
        {
            RuleFor(x => x.GroupName)
                .NotEmpty().WithMessage("Group name required.");
        }
    }
}
