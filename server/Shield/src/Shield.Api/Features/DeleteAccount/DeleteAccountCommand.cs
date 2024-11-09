using MediatR;

namespace Shield.Api.Features.DeleteAccount;

public class DeleteAccountCommand : IRequest<DeleteAccountResponse>
{
    public string UserId { get; set; }
    
    public int SocialId { get; set; }
}