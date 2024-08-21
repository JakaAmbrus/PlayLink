using MediatR;

namespace Social.Application.Features.Posts.GetPostById
{
    public class GetPostByIdQuery : IRequest<GetPostByIdResponse>
    {
        public int PostId { get; set; }
        public int AuthUserId { get; set; }
        public IEnumerable<string> AuthUserRoles { get; set; }
    }
}
