using MediatR;
using Shared.Core.Security;
using Shield.Api.Common.Abstractions;
using Shield.Api.Common.Configurations;

namespace Shield.Api.Features.SignInGuest;

public class SignInGuestCommandHandler : IRequestHandler<SignInGuestCommand, SignInGuestResponse>
{
    private readonly IIdentityService _identityService;
    private readonly GuestsOptions _guestsOptions;
    private readonly ICacheService _cachingService;

    public SignInGuestCommandHandler(IIdentityService identityService, Settings settings, ICacheService cachingService)
    {
        _identityService = identityService;
        _cachingService = cachingService;
        _guestsOptions = settings.Guests;
    }

    public async Task<SignInGuestResponse> Handle(SignInGuestCommand request, CancellationToken cancellationToken)
    {
        var guestAccounts = GetGuestAccountsForRole(request.Role);
        
        var selectedGuest = _cachingService.GetLeastRecentlyUsedGuest(request.Role, guestAccounts);
        
        var token = await _identityService.SignInAsync(selectedGuest.Email, selectedGuest.Password);
        
        _cachingService.UpdateLastUsedGuest(request.Role, selectedGuest.Email);
        
        return new SignInGuestResponse
        {
            Token = token,
        };
    }
    
    private List<GuestAccount> GetGuestAccountsForRole(string role)
    {
        return role switch
        {
            Roles.Member => _guestsOptions.Members,
            Roles.Moderator => _guestsOptions.Moderators,
            Roles.Admin => _guestsOptions.Admins,
            _ => throw new ArgumentException("Not a valid role"),
        };
    }
}