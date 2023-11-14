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

        public UploadPostCommandHandler(DataContext context,
            IAuthenticatedUserService authenticatedUserService)
        {
            _context = context;
            _authenticatedUserService = authenticatedUserService;
        }

        public async Task<UploadPostResponse> Handle(UploadPostCommand request, CancellationToken cancellationToken)
        {

            var post = new Post
            {
                AppUserId = _authenticatedUserService.UserId,
                Description = request.PostContentDto.Description,
                PhotoUrl = request.PostContentDto.PhotoUrl,        
            };

            _context.Posts.Add(post);

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
                    AppUserId = post.AppUserId,
                    PostId = post.PostId,
                    Description = post.Description,
                    PhotoUrl = post.PhotoUrl,
                    DatePosted = post.DatePosted,
                },
            };
        }
    }
}
