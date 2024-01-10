using BlazorMatchGame.Data;

namespace BlazorMatchGame.Services
{
    public interface ILeaderboardService
    {
        Task<User> CreatePlayer(User user, string username);
        Task UpdatePlayer(User user, int value);
        Task<List<User>> GetLeaderboard();
    }
}
