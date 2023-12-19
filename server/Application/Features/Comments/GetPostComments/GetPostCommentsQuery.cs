using Application.Utils;
using MediatR;

namespace Application.Features.Comments.GetComments
{
    public class GetPostCommentsQuery : IRequest<GetPostCommentsResponse>
    {
        public PaginationParams Params { get; set; } = new PaginationParams();
        public int PostId { get; set; }
        public int AuthUserId { get; set; }
        public IEnumerable<string> AuthUserRoles { get; set; }
    }
}
