using FluentValidation;

namespace Social.Application.Features.MessageGroups.AddConnectionToGroup
{
    public class AddConnectionToGroupCommandValidator : AbstractValidator<AddConnectionToGroupCommand>
    {
        public AddConnectionToGroupCommandValidator()
        {
            RuleFor(x => x.GroupName)
                .NotEmpty().WithMessage("Group name required.");

            RuleFor(x => x.ConnectionDto)
                .NotNull().WithMessage("Connection required.");
        }
    }
}
