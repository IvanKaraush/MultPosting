using MultiPosting.Application.Dto;

namespace MultiPosting.Application.Interfaces;

public interface IYoutubeService
{
    Task<List<YouTubeChannelDto>> AddAccountAsync(string email);
}