using Application.Utils;
using FluentValidation;

namespace Application.Features.Posts.GetPostsByUser
{
    public class GetPostsByUserQueryValidator : AbstractValidator<GetPostsByUserQuery>
    {
        public GetPostsByUserQueryValidator()
        {
            RuleFor(x => x.Username)
                .NotEmpty().WithMessage("Username is required");

            RuleFor(x => x.Params)
                .NotEmpty().WithMessage("Pagination parameters are required.");

            RuleFor(x => x.AuthUserId)
                .NotEmpty().WithMessage("Authenticated user Id required.");

            RuleFor(x => x.AuthUserRoles)
                .Must(ValidationUtils.BeValidRole).WithMessage("Invalid user role.");
        }
    }
}
