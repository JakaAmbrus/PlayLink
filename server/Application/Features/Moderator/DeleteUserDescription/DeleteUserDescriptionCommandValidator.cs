using FluentValidation;

namespace Application.Features.Moderator.DeleteUserDescription
{
    public class DeleteUserDescriptionCommandValidator : AbstractValidator<DeleteUserDescriptionCommand>
    {
        public DeleteUserDescriptionCommandValidator()
        {
            RuleFor(x => x.Username)
                .NotEmpty().WithMessage("Username required.");
        }      
    }
}
