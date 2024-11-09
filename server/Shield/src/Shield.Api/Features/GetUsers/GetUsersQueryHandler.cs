using MediatR;
using Shield.Api.Common.Abstractions;

namespace Shield.Api.Features.GetUsers;

public class GetUsersQueryHandler : IRequestHandler<GetUsersQuery, GetUsersResponse>
{
    private readonly IIdentityService _identityService;
    private readonly IFirestoreDbContext _firestoreDbContext;

    public GetUsersQueryHandler(IIdentityService identityService, IFirestoreDbContext firestoreDbContext)
    {
        _identityService = identityService;
        _firestoreDbContext = firestoreDbContext;
    }

    public async Task<GetUsersResponse> Handle(GetUsersQuery request, CancellationToken cancellationToken)
    {
        return new GetUsersResponse();
    }
}