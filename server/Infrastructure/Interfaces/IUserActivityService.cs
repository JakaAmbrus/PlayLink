namespace Infrastructure.Interfaces
{
    public interface IUserActivityService
    {
        Task UpdateLastActiveAsync(int userId);
    }
}
