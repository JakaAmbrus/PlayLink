using FluentValidation;

namespace Shield.Api.Features.GetUsers;

public class GetUsersQueryValidator : AbstractValidator<GetUsersQuery>
{
    public GetUsersQueryValidator()
    {
        RuleFor(x => x.PagedLimit)
            .NotEmpty();
    }
}