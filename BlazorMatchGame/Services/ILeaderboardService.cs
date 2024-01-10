using BlazorMatchGame.Data;

namespace BlazorMatchGame.Services
{
    public interface ILeaderboardService
    {
        Task CreatePlayer(User user, string username);
        Task UpdatePlayer(User user, int value);
        Task<List<User>> GetLeaderboard();
    }
}
