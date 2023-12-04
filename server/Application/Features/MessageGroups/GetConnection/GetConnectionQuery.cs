using Domain.Entities;
using MediatR;

namespace Application.Features.MessageGroups.GetConnection
{
    public class GetConnectionQuery : IRequest<Connection>
    {
        public string ConnectionId { get; set; }
    }
}
