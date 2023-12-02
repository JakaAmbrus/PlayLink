using Application.Exceptions;
using Application.Interfaces;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Application.Features.Posts.GetUserPostPhotos
{
    public class GetUserPostPhotosQueryHandler : IRequestHandler<GetUserPostPhotosQuery, GetUserPostPhotosResponse>
    {
        private readonly IApplicationDbContext _context;

        public GetUserPostPhotosQueryHandler(IApplicationDbContext context) 
        { 
            _context = context;
        }

        public async Task<GetUserPostPhotosResponse> Handle(GetUserPostPhotosQuery request, CancellationToken cancellationToken)
        {
            var user = await _context.Users
                .Include(user => user.Posts)
                .SingleOrDefaultAsync(user => user.UserName == request.Username, cancellationToken)
                ?? throw new NotFoundException($"User with username {request.Username} does not exist");

            var photos = user.Posts
                .Where(post => !string.IsNullOrEmpty(post.PhotoUrl))
                .Select(post => post.PhotoUrl)
                .ToList();

            if (!string.IsNullOrEmpty(user.ProfilePictureUrl))
            {
                // includes the profile picture as the first photo in the gallery
                photos.Insert(0, user.ProfilePictureUrl);
            }

            return new GetUserPostPhotosResponse { Photos = photos };
            
        }
    }
}
