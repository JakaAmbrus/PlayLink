using Application.Exceptions;
using Application.Interfaces;
using Domain.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Application.Features.Likes.LikePost;
public class LikePostCommandHandler : IRequestHandler<LikePostCommand, LikePostResponse>
{
    private readonly IApplicationDbContext _context;

    public LikePostCommandHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<LikePostResponse> Handle(LikePostCommand request, CancellationToken cancellationToken)
    {

        var post = await _context.Posts.FindAsync(new object[] { request.PostId }, cancellationToken)
            ?? throw new NotFoundException("Post not found");

        var existingLike = await _context.Likes
            .AnyAsync(l => l.PostId == request.PostId && l.AppUserId == request.AuthUserId);

        if (existingLike)
        {
            throw new BadRequestException("You have already liked this post");
        }

        var like = new Like
        {
            AppUserId = request.AuthUserId,
            PostId = request.PostId
        };

        post.LikesCount++;
        _context.Likes.Add(like);

        await _context.SaveChangesAsync(cancellationToken);

        return new LikePostResponse { Liked = true };
    }
}
