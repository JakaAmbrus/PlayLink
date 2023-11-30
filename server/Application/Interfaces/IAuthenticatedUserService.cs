namespace Application.Interfaces
{
    public interface IAuthenticatedUserService
    {
        int UserId { get; }
        IEnumerable<string> UserRoles { get; }
    }
}
