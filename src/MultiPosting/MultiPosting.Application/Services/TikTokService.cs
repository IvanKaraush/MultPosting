using MultiPosting.Application.Dto;
using MultiPosting.Application.Interfaces;

namespace MultiPosting.Application.Services;

public class TikTokService : ISocialMediaService
{
    public Task<ICollection<UserResourceDto>> GetUserResourceAsync(string login)
    {
        throw new NotImplementedException();
    }
}