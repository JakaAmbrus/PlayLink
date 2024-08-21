using MediatR;

namespace Social.Application.Features.Users.GetUserByUsername
{
    public class GetUserByUsernameQuery : IRequest<GetUserByUsernameResponse>
    {
        public string Username { get; set; }
        public int AuthUserId { get; set; }
        public IEnumerable<string> AuthUserRoles { get; set; }
    }
}
