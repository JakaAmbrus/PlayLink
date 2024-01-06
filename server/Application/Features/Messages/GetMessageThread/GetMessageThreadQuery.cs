using MediatR;

namespace Application.Features.Messages.GetMessageThread
{
    public class GetMessageThreadQuery : IRequest<GetMessageThreadResponse>
    {
        public string ProfileUsername { get; set; }
        public int AuthUserId { get; set; }
    }
}
