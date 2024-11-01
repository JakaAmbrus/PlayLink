using MediatR;
using Social.Application.Interfaces;
using Social.Domain.Exceptions;

namespace Social.Application.Features.Users.DeleteUserById;

public class DeleteUserByIdCommandHandler : IRequestHandler<DeleteUserByIdCommand, DeleteUserByIdResponse>
{
    private readonly ISocialDbContext _context;

    public DeleteUserByIdCommandHandler(ISocialDbContext context)
    {
        _context = context;
    }

    public async Task<DeleteUserByIdResponse> Handle(DeleteUserByIdCommand request, CancellationToken cancellationToken)
    {
        var user = await _context.Users.FindAsync(request.UserId)
                   ?? throw new NotFoundException("User not found");

        _context.Users.Remove(user);
        await _context.SaveChangesAsync(cancellationToken);

        return new DeleteUserByIdResponse();
    }
}