using FluentValidation;

namespace Social.Application.Features.MessageGroups.AddGroup
{
    public class AddGroupCommandValidator : AbstractValidator<AddGroupCommand>
    {
        public AddGroupCommandValidator()
        {
            RuleFor(x => x.GroupName)
                .NotEmpty().WithMessage("Group name required.");
        }
    }
}
