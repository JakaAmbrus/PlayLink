using FluentValidation;
using Social.Application.Utils;

namespace Social.Application.Features.Comments.GetComments
{
    public class GetPostCommentsQueryValidator : AbstractValidator<GetPostCommentsQuery>
    {
        public GetPostCommentsQueryValidator()
        {
            RuleFor(x => x.Params)
                .NotNull().WithMessage("Params required.");

            RuleFor(x => x.Params.PageNumber)
                .GreaterThan(0).WithMessage("Page Number must be greater than 0.");

            RuleFor(x => x.Params.PageSize)
                .GreaterThan(0).WithMessage("Page Size must be greater than 0.");

            RuleFor(x => x.PostId)
                .NotEmpty().WithMessage("PostId required.");

            RuleFor(x => x.AuthUserId)
                .NotEmpty().WithMessage("AuthUserId required.");

            RuleFor(x => x.AuthUserRoles)
                .Must(ValidationUtils.IsValidRole).WithMessage("Invalid role detected.");
        }
    }
}
