using FluentValidation;

namespace Application.Features.Moderator.DeleteUserPhoto
{
    public class DeleteUserPhotoCommandValidator : AbstractValidator<DeleteUserPhotoCommand>
    {
        public DeleteUserPhotoCommandValidator()
        {
            RuleFor(x => x.Username)
                .NotEmpty().WithMessage("Username is required.");
        }
    }
}
