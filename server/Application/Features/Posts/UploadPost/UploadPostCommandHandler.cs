using Application.Features.Posts.Common;
using Domain.Entities;
using FluentValidation;
using Infrastructure.Data;
using MediatR;

namespace Application.Features.Posts.UploadPost
{
    public class UploadPostCommandHandler : IRequestHandler<UploadPostCommand, UploadPostResponse>
    {
        private readonly DataContext _context;
        private readonly IValidator<UploadPostCommand> _validator;

        public UploadPostCommandHandler(DataContext context, IValidator<UploadPostCommand> validator)
        {
            _context = context;
            _validator = validator;
        }

        public async Task<UploadPostResponse> Handle(UploadPostCommand request, CancellationToken cancellationToken)
        {
            var validationResult = await _validator.ValidateAsync(request);

            if (!validationResult.IsValid)
            {
                var errors = validationResult.Errors.Select(error => error.ErrorMessage).ToList();

                return new UploadPostResponse
                {
                    PostId = null,
                    PostDto = null,
                    Errors = errors,
                    Success = false
                };
            }

            var post = new Post
            {
                    Description = request.PostDto.Description.Trim(),
                    PhotoUrl = request.PostDto.PhotoUrl,        
            };

            try
            {
                _context.Posts.Add(post);
                await _context.SaveChangesAsync(cancellationToken);
            }
            catch
            {
                return new UploadPostResponse
                {
                    PostId = null,
                    PostDto = null,
                    Errors = new List<string> { "An error occurred while saving the post." },
                    Success = false
                };
            }

            return new UploadPostResponse
            {
                PostId = post.PostId,
                PostDto = new PostDto
                {
                    Description = post.Description,
                    PhotoUrl = post.PhotoUrl,
                },
                Success = true
            };
        }
    }
}
