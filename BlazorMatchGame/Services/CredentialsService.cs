namespace BlazorMatchGame.Services
{
    public class CredentialsService
    {
        private readonly string APIKey;

        public CredentialsService(IConfiguration configuration) 
        {
            APIKey = configuration["Leaderboard:ApiKey"];
        }

        public string GetLeaderboardKey()
        {
            return APIKey;
        }

    }
}
