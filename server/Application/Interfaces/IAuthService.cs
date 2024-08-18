namespace Application.Interfaces
{
    public interface IAuthService
    {
        int GetCurrentUserId();
        IEnumerable<string> GetCurrentUserRoles();
        Task<string> GetUsernameByIdAsync(CancellationToken cancellationToken);
    }
}
