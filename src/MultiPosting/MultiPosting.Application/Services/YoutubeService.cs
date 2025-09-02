using Google.Apis.Auth.OAuth2;
using Google.Apis.Auth.OAuth2.Flows;
using Google.Apis.Auth.OAuth2.Responses;
using Google.Apis.Services;
using Google.Apis.YouTube.v3;
using MultiPosting.Application.Interfaces;

namespace MultiPosting.Application.Services;

public class YoutubeService : IYoutubeService
{
    private readonly YouTubeService _youtubeService;

    public YoutubeService(YouTubeService youtubeService)
    {
        _youtubeService = youtubeService;
    }

    public async Task AddAccountAsync()
    {
        var flow = new GoogleAuthorizationCodeFlow(new GoogleAuthorizationCodeFlow.Initializer
        {
            ClientSecrets = new ClientSecrets
            {
                ClientId = "",
                ClientSecret = ""
            }
        });

        var tokenResponse = new TokenResponse
        {
            AccessToken = "",
            RefreshToken = "",
            ExpiresInSeconds = 3600,                       // Set appropriately
            Scope = "https://www.googleapis.com/auth/youtube.readonly",
            TokenType = "Bearer"
        };

        var credential = new UserCredential(flow, "user", tokenResponse);

        var youtubeService = new YouTubeService(new BaseClientService.Initializer
        {
            HttpClientInitializer = credential,
            ApplicationName = "MultiPostingApp"
        });

        // Get all channels for logged-in user
        var request = youtubeService.Channels.List("id,snippet");
        request.Mine = true; // <--- get channels from authenticated user
        var response = await request.ExecuteAsync();

        foreach (var channel in response.Items)
        {
            Console.WriteLine($"Channel: {channel.Snippet.Title} (Id: {channel.Id})");
        }
    }
}