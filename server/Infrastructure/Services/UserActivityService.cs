using Infrastructure.Data;
using Application.Interfaces;

namespace Infrastructure.Services
{
    public class UserActivityService : IUserActivityService
    {
        private readonly DataContext _context;

        public UserActivityService(DataContext context)
        {
            _context = context;
        }

        public async Task UpdateLastActiveAsync(int userId)
        {
            var user = await _context.Users.FindAsync(userId);
            if (user != null)
            {
                user.LastActive = DateTime.UtcNow;
                await _context.SaveChangesAsync();
            }
        }
    }
    
}
