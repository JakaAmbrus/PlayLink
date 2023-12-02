using Application.Utils;
using FluentValidation;

namespace Application.Features.Posts.GetPosts
{
    public class GetPostsQueryValidator : AbstractValidator<GetPostsQuery>
    {
        public GetPostsQueryValidator()
        {
            RuleFor(x => x.Params)
                .NotNull().WithMessage("Params required.");

            RuleFor(x => x.AuthUserId)
                .NotEmpty().WithMessage("Authenticated user Id required.");

            RuleFor(x => x.AuthUserRoles)
                .Must(ValidationUtils.BeValidRole).WithMessage("Invalid user role.");
        }
    }
}
