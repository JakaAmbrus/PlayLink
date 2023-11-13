using Application.Features.Posts.Common;
using MediatR;

namespace Application.Features.Posts.UploadPost
{
    public class UploadPostCommand : IRequest<UploadPostResponse>
    {
        public PostDto PostContentDto { get; set; }
    }
}
