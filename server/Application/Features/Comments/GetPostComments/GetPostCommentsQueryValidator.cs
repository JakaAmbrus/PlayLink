using FluentValidation;

namespace Application.Features.Comments.GetComments
{
    public class GetPostCommentsQueryValidator : AbstractValidator<GetPostCommentsQuery>
    {
        public GetPostCommentsQueryValidator()
        {
            RuleFor(x => x.PostId)
                .GreaterThan(0).WithMessage("Invalid Post Id");
        }
    }
}
