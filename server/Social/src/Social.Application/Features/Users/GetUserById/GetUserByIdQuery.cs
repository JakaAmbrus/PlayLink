using MediatR;

namespace Social.Application.Features.Users.GetUserById
{
    public class GetUserByIdQuery : IRequest<GetUserByIdResponse>
    {
        public int Id { get; set; }
    }
}
