using System.Globalization;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Social.Application.Interfaces;
using Social.Domain.Entities;
using Social.Domain.Exceptions;

namespace Social.Application.Features.Authentication.UserRegistration
{
    public class UserRegistrationCommandHandler : IRequestHandler<UserRegistrationCommand, UserRegistrationResponse>
    {

        private readonly ISocialDbContext _context;
        private readonly ICacheInvalidationService _cacheInvalidationService;

        public UserRegistrationCommandHandler(ISocialDbContext context, ICacheInvalidationService cacheInvalidationService)
        {
            _context = context;
            _cacheInvalidationService = cacheInvalidationService;
        }

        public async Task<UserRegistrationResponse> Handle(UserRegistrationCommand request, CancellationToken cancellationToken)
        {
            bool userExists = await _context.Users.AnyAsync(u => u.Username == request.Username, cancellationToken);

            if (userExists)
            {
                throw new ConflictException("User already exists");
            }
            
            var user = new User
            {
                Username = request.Username,
                Gender = request.Gender,
                FullName = FormatPropertiesToTitleCase(request.FullName),
                Country = FormatPropertiesToTitleCase(request.Country),
                DateOfBirth = DateTime.SpecifyKind(request.DateOfBirth.ToDateTime(TimeOnly.MinValue), DateTimeKind.Utc),
                Created = DateTime.UtcNow,
            };

            _context.Users.Add(user);
            await _context.SaveChangesAsync(cancellationToken);
            
            _cacheInvalidationService.InvalidateSearchUserCache();
            _cacheInvalidationService.InvalidateNearestBirthdayUsersCache();

            var newUser = await _context.Users
                .AsNoTracking()
                .Where(x => x.Username == request.Username)
                .FirstOrDefaultAsync(cancellationToken);

            return new UserRegistrationResponse
            {
                SocialId = newUser.Id,
            };
            
        }
        
        private static string FormatPropertiesToTitleCase(string input)
        {
            var inputInfo = CultureInfo.CurrentCulture.TextInfo;
            return inputInfo.ToTitleCase(input.ToLower());
        }
    }
}
