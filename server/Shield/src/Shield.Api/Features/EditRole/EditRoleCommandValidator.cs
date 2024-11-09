using FluentValidation;

namespace Shield.Api.Features.EditRole;

public class EditRoleCommandValidator : AbstractValidator<EditRoleCommand>
{
    public EditRoleCommandValidator()
    {
        RuleFor(x => x.UserId)
            .NotEmpty();
    }
}