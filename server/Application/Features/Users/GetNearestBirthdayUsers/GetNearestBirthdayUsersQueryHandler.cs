using Application.Features.Users.Common;
using Application.Interfaces;
using Application.Utils;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;

namespace Application.Features.Users.GetNearestBirthdayUsers
{
    public class GetNearestBirthdayUsersQueryHandler : IRequestHandler<GetNearestBirthdayUsersQuery, GetNearestBirthdayUsersResponse>
    {
        private readonly IApplicationDbContext _context;
        private readonly IMemoryCache _memoryCache;

        public GetNearestBirthdayUsersQueryHandler(IApplicationDbContext context, IMemoryCache memoryCache)
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
                    u.UserName,
                    u.FullName,
                    u.ProfilePictureUrl,
                    u.Gender,
                    u.DateOfBirth
                })
                .ToListAsync(cancellationToken);

            var usersWithNearestBirthday = users
                .Select(u => new
                {
                    u.UserName,
                    u.FullName,
                    u.ProfilePictureUrl,
                    u.Gender,
                    u.DateOfBirth,
                    DaysUntilBirthday = DateTimeUtils.CalculateDaysUntilBirthday(DateOnly.FromDateTime(u.DateOfBirth), today)
                })
                .OrderBy(u => u.DaysUntilBirthday)
                .Take(3)
                .Select(u => new UserBirthdayDto
                {
                    Username = u.UserName,
                    FullName = u.FullName,
                    ProfilePictureUrl = u.ProfilePictureUrl,
                    Gender = u.Gender,
                    DateOfBirth = DateOnly.FromDateTime(u.DateOfBirth),
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
