using FluentValidation;

namespace Shield.Api.Features.DeleteUser;

public class DeleteUserCommandValidator : AbstractValidator<DeleteUserCommand>
{
    public DeleteUserCommandValidator()
    {
        RuleFor(x => x.ToString())
            .NotEmpty();
    }
}