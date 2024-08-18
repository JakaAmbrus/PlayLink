using Application.Features.Posts.Common;
using Application.Interfaces;
using Domain.Entities;
using Domain.Exceptions;
using MediatR;

namespace Application.Features.Posts.UploadPost
{
    public class UploadPostCommandHandler : IRequestHandler<UploadPostCommand, UploadPostResponse>
    {
        private readonly IApplicationDbContext _context;
        private readonly IPhotoService _photoService;
        private readonly ICacheInvalidationService _cacheInvalidationService;

        public UploadPostCommandHandler(IApplicationDbContext context, IPhotoService photoService, ICacheInvalidationService cacheInvalidationService)
        {
            _context = context;
            _photoService = photoService;
            _cacheInvalidationService = cacheInvalidationService;
        }

        public async Task<UploadPostResponse> Handle(UploadPostCommand request, CancellationToken cancellationToken)
        {

            var authUser = await _context.Users.FindAsync(new object[] { request.AuthUserId }, cancellationToken)
                ?? throw new NotFoundException("Authenticated user not found");

            var newPost = new Post
            {
                AppUserId = request.AuthUserId,
                Description = request.PostContentDto.Description,

            };

            if (request.PostContentDto.PhotoFile != null && request.PostContentDto.PhotoFile.Length > 0)
            {
                var uploadResult = await _photoService.AddPhotoAsync(request.PostContentDto.PhotoFile, "post");

                if (uploadResult.Error == null)
                {
                    newPost.PhotoUrl = uploadResult.Url.ToString();
                    newPost.PhotoPublicId = uploadResult.PublicId.ToString();

                    _cacheInvalidationService.InvalidateUserPhotosCache(authUser.UserName);
                }
                else
                {
                    throw new ServerErrorException(uploadResult.Error);
                }
            }

            _context.Posts.Add(newPost);
            await _context.SaveChangesAsync(cancellationToken);

            return new UploadPostResponse
            {
                PostDto = new PostDto
                {
                    PostId = newPost.PostId,
                    AppUserId = newPost.AppUserId,
                    Username = authUser.UserName,
                    FullName = authUser.FullName,
                    ProfilePictureUrl = authUser.ProfilePictureUrl,
                    Gender = authUser.Gender,
                    Description = newPost.Description,
                    PhotoUrl = newPost.PhotoUrl,
                    DatePosted = newPost.DatePosted,
                    LikesCount = newPost.LikesCount,
                    CommentsCount = newPost.CommentsCount,
                    IsLikedByCurrentUser = false,
                    IsAuthorized = true
                },
            };
        }
    }
}
