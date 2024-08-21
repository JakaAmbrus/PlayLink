using MediatR;

namespace Social.Application.Features.Users.GetUserIdFromUsername
{
    public class GetUserIdByUsernameQuery : IRequest<int>
    {
        public string Username { get; set; }
    }
}
