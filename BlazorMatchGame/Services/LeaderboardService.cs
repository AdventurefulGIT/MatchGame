using BlazorMatchGame.Data;
using System.IO;
using System.Text;
using System.Text.Json;

namespace BlazorMatchGame.Services
{
    public class LeaderboardService : ILeaderboardService
    {
        private readonly HttpClient _httpClient;

        public LeaderboardService(HttpClient httpClient, CredentialsService credentialsService)
        {
            _httpClient = httpClient;
            _httpClient.BaseAddress = new Uri($"https://keepthescore.com/api/{credentialsService.GetLeaderboardKey()}/");
        }

        public async Task<User> CreatePlayer(User player, string username)
        {
            player.Username = username;

            var playerData = new { name = username };
            var content = new StringContent(JsonSerializer.Serialize(playerData), Encoding.UTF8, "application/json");
            var response = await _httpClient.PostAsync("player/", content);

            if (response.IsSuccessStatusCode)
            {
                using (var stream = await response.Content.ReadAsStreamAsync())
                {
                    using (JsonDocument document = await JsonDocument.ParseAsync(stream))
                    {
                        player.ID = document.RootElement.GetProperty("player").GetProperty("id").GetInt32();
                        return player;
                    }
                }
            } 
            else
            {
                throw new Exception("Request failed.");
            }
        }

        public async Task UpdatePlayer(User user, int value)
        {
            if(!user.IsGuest && user.ID == 0)
            {
                // Creating the player when they have a score since KeepTheScore automatically assigns a score of 0 seconds which would make them the best player.
                await CreatePlayer(user, user.Username);
            }

            // If they don't have a score yet or their score was worse than their personal best, return.
            // If they're a guest, don't save the score, return.
            if((user.Score != 0 && user.Score <= value) || user.IsGuest)
            {
                return;
            }

            var playerData = new
            {
                player_id = user.ID,
                score = value - user.Score // Need to substract the score since API only permits increments.
            };

            var content = new StringContent(JsonSerializer.Serialize(playerData), Encoding.UTF8, "application/json");
            var response = await _httpClient.PostAsync("score/", content);

            if (response.IsSuccessStatusCode)
            {
                user.Score = value;
                return;
            }
            else
            {
                throw new Exception("Request failed.");
            }
        }

        public async Task<List<User>> GetLeaderboard()
        {
            List<User> scoreboard = new List<User>();
            var response = await _httpClient.GetAsync("board/");

            if (response.IsSuccessStatusCode)
            {
                using (var stream = await response.Content.ReadAsStreamAsync())
                {
                    using (JsonDocument document = await JsonDocument.ParseAsync(stream))
                    {
                        foreach(var player in document.RootElement.GetProperty("players").EnumerateArray())
                        {
                            scoreboard.Add(new User
                            {
                                ID = player.GetProperty("id").GetInt32(),
                                Username = player.GetProperty("name").GetString(),
                                Score = player.GetProperty("score").GetInt32(),
                                Rank = player.GetProperty("rank").GetInt32()
                            });
                        }
                    }
                }
                // Using Ranking system provided by KeepTheScore to match their leaderboard.
                return scoreboard.OrderBy(u => u.Rank).ToList();
            }
            else
            {
                throw new Exception("Request failed.");
            }
        }
    }
}
