using Application.Features.Users.Common;
using Application.Interfaces;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Application.Utils;
using Microsoft.Extensions.Caching.Memory;

namespace Application.Features.Users.GetNearestBirthdayUsers
{
    public class GetNearestBirthdayUsersQueryHandler : IRequestHandler<GetNearestBirthdayUsersQuery, GetNearestBirthdayUsersResult>
    {
        private readonly IApplicationDbContext _context;
        private readonly IMemoryCache _memoryCache;
        private readonly ICacheKeyService _cacheKeyService;

        public GetNearestBirthdayUsersQueryHandler(IApplicationDbContext context, IMemoryCache memoryCache, ICacheKeyService cacheKeyService)
        {
            _context = context;
            _memoryCache = memoryCache;
            _cacheKeyService = cacheKeyService;
        }

        public async Task<GetNearestBirthdayUsersResult> Handle(GetNearestBirthdayUsersQuery request, CancellationToken cancellationToken)
        {
            string cacheKey = _cacheKeyService.GenerateHashedKey($"Users:GetNearestBirthdayUsers");

            if (!_memoryCache.TryGetValue(cacheKey, out List<UserBirthdayDto> usersWithNearestBirthday))
            {
                var today = DateOnly.FromDateTime(DateTime.Today);

                var users = await _context.Users
                    .Select(u => new
                    {
                        u.UserName,
                        u.FullName,
                        u.ProfilePictureUrl,
                        u.Gender,
                        u.DateOfBirth
                    })
                    .ToListAsync(cancellationToken);

                usersWithNearestBirthday = users
                    .Select(u => new
                    {
                        u.UserName,
                        u.FullName,
                        u.ProfilePictureUrl,
                        u.Gender,
                        u.DateOfBirth,
                        DaysUntilBirthday = DateTimeExtensions.CalculateDaysUntilBirthday(u.DateOfBirth, today)
                    })
                    .OrderBy(u => u.DaysUntilBirthday)
                    .Take(3)
                    .Select(u => new UserBirthdayDto
                    {
                        Username = u.UserName,
                        FullName = u.FullName,
                        ProfilePictureUrl = u.ProfilePictureUrl,
                        Gender = u.Gender,
                        DateOfBirth = u.DateOfBirth,
                        DaysUntilBirthday = u.DaysUntilBirthday
                    })
                    .ToList();

                var cacheEntryOptions = new MemoryCacheEntryOptions
                {
                    AbsoluteExpirationRelativeToNow = TimeSpan.FromDays(1)
                };

                _memoryCache.Set(cacheKey, usersWithNearestBirthday, cacheEntryOptions);
            }

            return new GetNearestBirthdayUsersResult { Users = usersWithNearestBirthday };
        }
    }
}
