using Application.Utils;
using FluentValidation;

namespace Application.Features.Comments.GetComments
{
    public class GetPostCommentsQueryValidator : AbstractValidator<GetPostCommentsQuery>
    {
        public GetPostCommentsQueryValidator()
        {
            RuleFor(x => x.PostId)
                .NotEmpty().WithMessage("PostId required.");

            RuleFor(x => x.AuthUserId)
                .NotEmpty().WithMessage("AuthUserId required.");

            RuleFor(x => x.AuthUserRoles)
                .Must(ValidationUtils.BeValidRole).WithMessage("Invalid role detected.");
        }
    }
}
