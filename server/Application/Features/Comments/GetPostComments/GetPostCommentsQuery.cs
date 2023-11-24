using MediatR;

namespace Application.Features.Comments.GetComments
{
    public class GetPostCommentsQuery : IRequest<GetPostCommentsResponse>
    {
        public int PostId { get; set; }
    }
}
