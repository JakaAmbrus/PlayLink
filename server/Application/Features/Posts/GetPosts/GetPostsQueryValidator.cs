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

            RuleFor(x => x.Params.PageNumber)
                .GreaterThan(0).WithMessage("Page Number must be greater than 0.");

            RuleFor(x => x.Params.PageSize)
                .GreaterThan(0).WithMessage("Page Size must be greater than 0.");

            RuleFor(x => x.AuthUserId)
                .NotEmpty().WithMessage("Authenticated user Id required.");

            RuleFor(x => x.AuthUserRoles)
                .Must(ValidationUtils.IsValidRole).WithMessage("Invalid user role.");
        }
    }
}
