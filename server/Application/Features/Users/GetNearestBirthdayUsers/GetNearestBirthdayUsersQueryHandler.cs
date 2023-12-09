using Application.Features.Users.Common;
using Application.Interfaces;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Application.Utils;

namespace Application.Features.Users.GetNearestBirthdayUsers
{
    public class GetNearestBirthdayUsersQueryHandler : IRequestHandler<GetNearestBirthdayUsersQuery, GetNearestBirthdayUsersResult>
    {
        private readonly IApplicationDbContext _context;

        public GetNearestBirthdayUsersQueryHandler(IApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<GetNearestBirthdayUsersResult> Handle(GetNearestBirthdayUsersQuery request, CancellationToken cancellationToken)
        {
            var today = DateOnly.FromDateTime(DateTime.Today);

            var users = await _context.Users
                .Where(u => u.Id != request.AuthUserId)
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

            return new GetNearestBirthdayUsersResult { Users = usersWithNearestBirthday };
        }
    }
}
