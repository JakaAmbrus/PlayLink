using MediatR;
using Social.Application.Utils;

namespace Social.Application.Features.Posts.GetPostsByUser
{
    public class GetPostsByUserQuery : IRequest<GetPostsByUserResponse>
    {
        public string Username { get; set; }
        public PaginationParams Params { get; set; }
        public int AuthUserId { get; set; }
        public IEnumerable<string> AuthUserRoles { get; set; }
    }
}
