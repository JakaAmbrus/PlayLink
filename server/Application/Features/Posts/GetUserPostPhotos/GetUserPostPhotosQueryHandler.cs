using Application.Exceptions;
using Application.Interfaces;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;

namespace Application.Features.Posts.GetUserPostPhotos
{
    public class GetUserPostPhotosQueryHandler : IRequestHandler<GetUserPostPhotosQuery, GetUserPostPhotosResponse>
    {
        private readonly IApplicationDbContext _context;
        private readonly IMemoryCache _memoryCache;

        public GetUserPostPhotosQueryHandler(IApplicationDbContext context, IMemoryCache memoryCache) 
        { 
            _context = context;
            _memoryCache = memoryCache;
        }

        public async Task<GetUserPostPhotosResponse> Handle(GetUserPostPhotosQuery request, CancellationToken cancellationToken)
        {
            string cacheKey = $"Photos:GetUserPhotos-{request.Username}";

            if (!_memoryCache.TryGetValue(cacheKey, out List<string> photos))
            {
                var user = await _context.Users
                    .Include(user => user.Posts)
                    .SingleOrDefaultAsync(user => user.UserName == request.Username, cancellationToken)
                    ?? throw new NotFoundException($"User with username {request.Username} does not exist");

                photos = user.Posts
                    .Where(post => !string.IsNullOrEmpty(post.PhotoUrl))
                    .Select(post => post.PhotoUrl)
                    .ToList();

                if (!string.IsNullOrEmpty(user.ProfilePictureUrl))
                {
                    // includes the profile picture as the first photo in the gallery
                    photos.Insert(0, user.ProfilePictureUrl);
                }

                var cacheEntryOptions = new MemoryCacheEntryOptions
                {
                    AbsoluteExpirationRelativeToNow = TimeSpan.FromDays(1),
                };

                _memoryCache.Set(cacheKey, photos, cacheEntryOptions);
            }
         

            return new GetUserPostPhotosResponse { Photos = photos };          
        }
    }
}
