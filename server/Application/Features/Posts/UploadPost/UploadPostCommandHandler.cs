﻿using Application.Exceptions;
using Application.Features.Posts.Common;
using Domain.Entities;
using Infrastructure.Data;
using Infrastructure.Interfaces;
using MediatR;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory.Database;

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
            var newPost = new Post
            {
                AppUserId = _authenticatedUserService.UserId,
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
                    AppUserId = newPost.AppUserId,
                    PostId = newPost.PostId,
                    Description = newPost.Description,
                    PhotoUrl = newPost.PhotoUrl,
                    DatePosted = newPost.DatePosted,
                },
            };
        }
    }
}
