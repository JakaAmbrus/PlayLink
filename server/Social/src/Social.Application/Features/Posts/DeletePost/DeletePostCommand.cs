using MediatR;

namespace Social.Application.Features.Posts.DeletePost
{
    public class DeletePostCommand : IRequest<DeletePostResponse>
    {
        public int PostId { get; set; }
        public int AuthUserId { get; set; }
        public IEnumerable<string> AuthUserRoles { get; set; }
    }
}
