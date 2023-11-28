using Application.Utils;
using MediatR;

namespace Application.Features.Posts.GetPostsByUser
{
    public class GetPostsByUserQuery : IRequest<GetPostsByUserResponse>
    {
        public string Username { get; set; }
        public PaginationParams Params { get; set; }
    }
}
