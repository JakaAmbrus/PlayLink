using FluentValidation;

namespace Application.Features.Friends.GetFriendRequests
{
    public class GetFriendRequestsQueryValidator : AbstractValidator<GetFriendRequestsQuery>
    {
        public GetFriendRequestsQueryValidator()
        {
            RuleFor(x => x.AuthUserId)
                .NotEmpty().WithMessage("Auth user Id required.");
        }
    }
}
