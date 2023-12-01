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
                .SingleOrDefaultAsync(user => user.UserName == request.Username);

            if (user == null) throw new NotFoundException($"User with username {request.Username} does not exist");

            var photos = user.Posts
                .Where(post => !string.IsNullOrEmpty(post.PhotoUrl))
                .Select(post => post.PhotoUrl)
                .ToList();

            if (!string.IsNullOrEmpty(user.ProfilePictureUrl))
            {
                photos.Insert(0, user.ProfilePictureUrl); // I want to add the profile picture to the gallery
            }

            return new GetUserPostPhotosResponse { Photos = photos };
            
        }
    }
}
