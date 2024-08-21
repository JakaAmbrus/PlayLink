using Social.Domain.Entities;
using MediatR;

namespace Social.Application.Features.MessageGroups.GetConnection
{
    public class GetConnectionQuery : IRequest<Connection>
    {
        public string ConnectionId { get; set; }
    }
}
