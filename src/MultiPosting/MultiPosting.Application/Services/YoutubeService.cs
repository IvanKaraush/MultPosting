using System.Text.Json;
using Google.Apis.Auth.OAuth2;
using Google.Apis.Auth.OAuth2.Flows;
using Google.Apis.Auth.OAuth2.Responses;
using Google.Apis.Services;
using Google.Apis.YouTube.v3;
using Microsoft.Extensions.Options;
using MultiPosting.Application.Dto;
using MultiPosting.Application.Interfaces;
using MultiPosting.Application.Options;
using Share.Application.Options;
using Shared.Infrastructure.Interfaces;

namespace MultiPosting.Application.Services;

public class YoutubeService : IYoutubeService
{
    private readonly MultiPostingOptions _multiPostingOptions;
    private readonly GoogleOptions _googleOptions;
    private readonly IHttpProvider _httpProvider;

    public YoutubeService(IHttpProvider httpProvider, IOptions<MultiPostingOptions> multiPostingOptions,
        IOptions<GoogleOptions> googleOptions)
    {
        _httpProvider = httpProvider;
        _multiPostingOptions = multiPostingOptions.Value;
        _googleOptions = googleOptions.Value;
    }

    public async Task<List<YouTubeChannelDto>> AddAccountAsync(string email)
    {
        var token = await _httpProvider.SendGetAsync<AccessTokenResponse>(
            $"{_multiPostingOptions.IdentityServerUrl}/api/accesstoken?email={email}", CancellationToken.None, options: new JsonSerializerOptions
            {
                PropertyNamingPolicy = JsonNamingPolicy.CamelCase
            });
        if (token == null)
        {
            // Get all channels using only youtube credentials
            return [];
        }

        var flow = new GoogleAuthorizationCodeFlow(new GoogleAuthorizationCodeFlow.Initializer
        {
            ClientSecrets = new ClientSecrets
            {
                ClientId = _googleOptions.ClientId,
                ClientSecret = _googleOptions.ClientSecret,
            },
            Scopes = new[] 
            { 
                "https://www.googleapis.com/auth/youtube.readonly",
                "https://www.googleapis.com/auth/youtube"
            }
        });

        var tokenResponse = new TokenResponse
        {
            AccessToken = token.Token,
            RefreshToken = token.RefreshToken,
            ExpiresInSeconds = 3600,
            TokenType = "Bearer"
        };

        var credential = new UserCredential(flow, "user", tokenResponse);

        var youtubeService = new YouTubeService(new BaseClientService.Initializer
        {
            HttpClientInitializer = credential,
            ApplicationName = "MultiPostingApp"
        });

        var request = youtubeService.Channels.List("id,snippet");
        request.Mine = true;
        var response = await request.ExecuteAsync();
        var youtubeChannels = new List<YouTubeChannelDto>();
        foreach (var channel in response.Items)
        {
            youtubeChannels.Add(new YouTubeChannelDto
            {
                Title = channel.Snippet.Title,
                Description = channel.Snippet.Description,
            });
        }

        return youtubeChannels;
    }
}