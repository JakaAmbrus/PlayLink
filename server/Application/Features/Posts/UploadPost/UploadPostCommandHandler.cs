using Application.Exceptions;
using Application.Features.Posts.Common;
using Domain.Entities;
using Infrastructure.Data;
using Infrastructure.Interfaces;
using MediatR;

namespace Application.Features.Posts.UploadPost
{
    public class UploadPostCommandHandler : IRequestHandler<UploadPostCommand, UploadPostResponse>
    {
        private readonly DataContext _context;
        private readonly IAuthenticatedUserService _authenticatedUserService;
        private readonly IPhotoService _photoService;

        public UploadPostCommandHandler(DataContext context,
            IAuthenticatedUserService authenticatedUserService,
            IPhotoService photoService)
        {
            _context = context;
            _authenticatedUserService = authenticatedUserService;
            _photoService = photoService;
        }

        public async Task<UploadPostResponse> Handle(UploadPostCommand request, CancellationToken cancellationToken)
        {

            int currentUserId = _authenticatedUserService.UserId;

            var currentUser = await _context.Users.FindAsync(currentUserId);
            if (currentUser == null)
            {
                   throw new NotFoundException("User not found");
            }

            var newPost = new Post
            {
                AppUserId = currentUserId,
                Description = request.PostContentDto.Description,

            };

            if (request.PostContentDto.PhotoFile != null && request.PostContentDto.PhotoFile.Length > 0)
            {
                var uploadResult = await _photoService.AddPhotoAsync(request.PostContentDto.PhotoFile, "post");

                if (uploadResult.Error == null)
                {
                    newPost.PhotoUrl = uploadResult.Url.ToString();
                    newPost.PhotoPublicId = uploadResult.PublicId.ToString();
                }
                else
                {
                    throw new ServerErrorException(uploadResult.Error.Message);
                }
            }

            _context.Posts.Add(newPost);

            try
            {
                await _context.SaveChangesAsync(cancellationToken);
            }
            catch
            {
                throw new ServerErrorException("Problem saving post");
            }

            return new UploadPostResponse
            {
                PostDto = new PostDto
                {
                    PostId = newPost.PostId,
                    AppUserId = newPost.AppUserId,
                    Username = currentUser.UserName,
                    FullName = currentUser.FullName,
                    ProfilePictureUrl = currentUser.ProfilePictureUrl,
                    Gender = currentUser.Gender,
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
