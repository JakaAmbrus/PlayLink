using FluentValidation;

namespace Application.Features.Posts.GetPostsByUser
{
    public class GetPostsByUserQueryValidator : AbstractValidator<GetPostsByUserQuery>
    {
        public GetPostsByUserQueryValidator()
        {
            RuleFor(x => x.Username)
                .NotEmpty().WithMessage("Username is required");
        }
    }
}
