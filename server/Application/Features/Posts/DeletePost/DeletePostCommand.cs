using MediatR;

namespace Application.Features.Posts.DeletePost
{
    public class DeletePostCommand : IRequest<DeletePostResponse>
    {
        public int PostId { get; set; }
    }
}
