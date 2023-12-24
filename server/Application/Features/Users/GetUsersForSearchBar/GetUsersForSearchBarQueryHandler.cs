using Application.Features.Users.Common;
using Application.Interfaces;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;

namespace Application.Features.Users.GetUsersForSearchBar
{
    public class GetUsersForSearchBarQueryHandler : IRequestHandler<GetUsersForSearchBarQuery, GetUsersForSearchBarResponse>
    {
        private readonly IApplicationDbContext _context;
        private readonly IMemoryCache _memoryCache;
        private readonly ICacheKeyService _cacheKeyService;

        public GetUsersForSearchBarQueryHandler(IApplicationDbContext context, IMemoryCache memoryCache, ICacheKeyService cacheKeyService)
        {
            _context = context;
            _memoryCache = memoryCache;
            _cacheKeyService = cacheKeyService;
        }
        public async Task<GetUsersForSearchBarResponse> Handle(GetUsersForSearchBarQuery request, CancellationToken cancellationToken)
        {
            string cacheKey = _cacheKeyService.GenerateHashedKey("Users:GetSearchUsers");

            if (!_memoryCache.TryGetValue(cacheKey, out List<SearchUserDto> searchUsers))
            {
                searchUsers = await _context.Users
                  .OrderByDescending(u => u.LastActive)
                  .Select(u => new SearchUserDto
                  {
                      AppUserId = u.Id,
                      Username = u.UserName,
                      FullName = u.FullName,
                      ProfilePictureUrl = u.ProfilePictureUrl,
                      Gender = u.Gender,
                  }).ToListAsync(cancellationToken);

                var cacheEntryOptions = new MemoryCacheEntryOptions
                {
                    AbsoluteExpirationRelativeToNow = TimeSpan.FromMinutes(10)
                };

                _memoryCache.Set(cacheKey, searchUsers, cacheEntryOptions);
            }

            return new GetUsersForSearchBarResponse
            {
                Users = searchUsers
            };
        }
    }
}
