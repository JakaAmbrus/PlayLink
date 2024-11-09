using MediatR;

namespace Shield.Api.Features.SignInGuest;

public class SignInGuestCommand : IRequest<SignInGuestResponse>
{
    public string Role { get; set; }
}