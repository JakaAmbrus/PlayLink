using MediatR;
using Social.Application.Features.Posts.Common;

namespace Social.Application.Features.Posts.UploadPost
{
    public class UploadPostCommand : IRequest<UploadPostResponse>
    {
        public PostContentDto PostContentDto { get; set; }
        public int AuthUserId { get; set; }
    }
}
