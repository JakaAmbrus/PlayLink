namespace Application.Interfaces
{
    public interface IAuthenticatedUserUsernameService
    {
        Task<string> GetUsernameByIdAsync();
    }
}
