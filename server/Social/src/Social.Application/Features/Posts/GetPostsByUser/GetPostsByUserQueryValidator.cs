using FluentValidation;
using Social.Application.Utils;

namespace Social.Application.Features.Posts.GetPostsByUser
{
    public class GetPostsByUserQueryValidator : AbstractValidator<GetPostsByUserQuery>
    {
        public GetPostsByUserQueryValidator()
        {
            RuleFor(x => x.Username)
                .NotEmpty().WithMessage("Username is required");

            RuleFor(x => x.Params)
                .NotEmpty().WithMessage("Pagination parameters are required.");

            RuleFor(x => x.Params.PageNumber)
                .GreaterThan(0).WithMessage("Page number must be greater than 0.");

            RuleFor(x => x.Params.PageSize)
                .GreaterThan(0).WithMessage("Page size must be greater than 0.");

            RuleFor(x => x.AuthUserId)
                .NotEmpty().WithMessage("Authenticated user Id required.");

            RuleFor(x => x.AuthUserRoles)
                .Must(ValidationUtils.IsValidRole).WithMessage("Invalid user role.");
        }
    }
}
