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
        var request = _youtubeService.Channels.List("id");
        request.ForUsername = "PatrickGod";
        var response = await request.ExecuteAsync();
    }
}