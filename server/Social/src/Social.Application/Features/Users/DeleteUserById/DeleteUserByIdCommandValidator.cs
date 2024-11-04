using FluentValidation;

namespace Social.Application.Features.Users.DeleteUserById;

public class DeleteUserByIdCommandValidator : AbstractValidator<DeleteUserByIdCommand>
{
    public DeleteUserByIdCommandValidator()
    {
        RuleFor(x => x.UserId)
            .NotEmpty();
    }
}