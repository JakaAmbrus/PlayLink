using Domain.Entities;
using MediatR;

namespace Application.Features.Users.GetUserById
{
    public class GetUserByIdQuery : IRequest<AppUser>
    {
        public int Id { get; set; }
        public GetUserByIdQuery(int id)
        {
            Id = id;
        }
    }
}
