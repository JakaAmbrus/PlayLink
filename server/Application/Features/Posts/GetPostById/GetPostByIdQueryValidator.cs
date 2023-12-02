using Application.Utils;
using FluentValidation;

namespace Application.Features.Posts.GetPostById
{
    public class GetPostByIdQueryValidator : AbstractValidator<GetPostByIdQuery>
    { 
        public GetPostByIdQueryValidator()
        {
            RuleFor(x => x.PostId)
                .NotEmpty().WithMessage("PostId required.");

            RuleFor(x => x.AuthUserId)
                .NotEmpty().WithMessage("Authenticated user Id required.");

            RuleFor(x => x.AuthUserRoles)
                .Must(ValidationUtils.BeValidRole).WithMessage("Invalid user role.");
        }
    }
}
