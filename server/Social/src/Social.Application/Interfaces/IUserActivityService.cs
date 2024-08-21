namespace Social.Application.Interfaces
{
    public interface IUserActivityService
    {
        Task UpdateLastActiveAsync(int userId);
    }
}
