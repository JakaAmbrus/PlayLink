using FluentValidation;

namespace Application.Features.Posts.GetUserPostPhotos
{
    public class GetUserPostPhotosQueryValidator : AbstractValidator<GetUserPostPhotosQuery>
    {
        public GetUserPostPhotosQueryValidator()
        {
            RuleFor(x => x.Username)
                .NotEmpty().WithMessage("Username is required");
        }
    }
}
