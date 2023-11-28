using MediatR;

namespace Application.Features.Messages.GetMessageThread
{
    public class GetMessageThreadQuery : IRequest<GetMessageThreadResponse>
    {
        public string RecipientUsername { get; set; }
    }
}
