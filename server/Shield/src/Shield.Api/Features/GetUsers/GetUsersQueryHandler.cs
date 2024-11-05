using MediatR;
using Shield.Api.Common.Abstractions;

namespace Shield.Api.Features.GetUsers;

public class GetUsersQueryHandler : IRequestHandler<GetUsersQuery, GetUsersResponse>
{
    private readonly IIdentityService _identityService;
    private readonly IFirebaseDbContext _firebaseDbContext;

    public GetUsersQueryHandler(IIdentityService identityService, IFirebaseDbContext firebaseDbContext)
    {
        _identityService = identityService;
        _firebaseDbContext = firebaseDbContext;
    }

    public async Task<GetUsersResponse> Handle(GetUsersQuery request, CancellationToken cancellationToken)
    {
        return new GetUsersResponse();
    }
}