using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;
using Social.Application.Features.Users.Common;
using Social.Application.Interfaces;
using Social.Application.Utils;

namespace Social.Application.Features.Users.GetNearestBirthdayUsers
{
    public class GetNearestBirthdayUsersQueryHandler : IRequestHandler<GetNearestBirthdayUsersQuery, GetNearestBirthdayUsersResponse>
    {
        private readonly ISocialDbContext _context;
        private readonly IMemoryCache _memoryCache;

        public GetNearestBirthdayUsersQueryHandler(ISocialDbContext context, IMemoryCache memoryCache)
        {
            _context = context;
            _memoryCache = memoryCache;
        }

        public async Task<GetNearestBirthdayUsersResponse> Handle(GetNearestBirthdayUsersQuery request, CancellationToken cancellationToken)
        {
            string cacheKey = "Users:GetNearestBirthdayUsers";

            if (_memoryCache.TryGetValue(cacheKey, out List<UserBirthdayDto> cachedUsers))
            {
                return new GetNearestBirthdayUsersResponse { Users = cachedUsers };
            }

            var today = DateOnly.FromDateTime(DateTime.Today);

            var users = await _context.Users
                .Select(u => new
                {
                    u.Username,
                    u.FullName,
                    u.ProfilePictureUrl,
                    u.Gender,
                    u.DateOfBirth
                })
                .ToListAsync(cancellationToken);

            var usersWithNearestBirthday = users
                .Select(u => new
                {
                    u.Username,
                    u.FullName,
                    u.ProfilePictureUrl,
                    u.Gender,
                    u.DateOfBirth,
                    DaysUntilBirthday = DateTimeUtils.CalculateDaysUntilBirthday(u.DateOfBirth, today)
                })
                .OrderBy(u => u.DaysUntilBirthday)
                .Take(3)
                .Select(u => new UserBirthdayDto
                {
                    Username = u.Username,
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

            return new GetNearestBirthdayUsersResponse { Users = usersWithNearestBirthday };
        }
    }
}
