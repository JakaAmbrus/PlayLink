using Application.Exceptions;
using Domain.Entities;
using Infrastructure.Data;
using Infrastructure.Interfaces;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Application.Features.Likes.LikePost;
public class LikePostCommandHandler : IRequestHandler<LikePostCommand, LikePostResponse>
{
    private readonly DataContext _context;
    private readonly IAuthenticatedUserService _authenticatedUserService;

    public LikePostCommandHandler(DataContext context, IAuthenticatedUserService authenticatedUserService)
    {
        _context = context;
        _authenticatedUserService = authenticatedUserService;
    }

    public async Task<LikePostResponse> Handle(LikePostCommand request, CancellationToken cancellationToken)
    {
        var currentUserId = _authenticatedUserService.UserId;

        var post = await _context.Posts.FindAsync(request.PostId) 
            ?? throw new NotFoundException("Post not found");

        var existingLike = await _context.Likes
            .AnyAsync(l => l.PostId == request.PostId && l.AppUserId == currentUserId);

        if (existingLike)
        {
            throw new BadRequestException("You have already liked this post");
        }

        var like = new Like
        {
            AppUserId = currentUserId,
            PostId = request.PostId
        };

        post.LikesCount++;
        _context.Likes.Add(like);

        await _context.SaveChangesAsync(cancellationToken);

        return new LikePostResponse { Liked = true };
    }
}
