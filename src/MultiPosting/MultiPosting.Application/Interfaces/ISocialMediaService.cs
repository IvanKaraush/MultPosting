using MultiPosting.Application.Dto;

namespace MultiPosting.Application.Interfaces;

public interface ISocialMediaService
{
    Task<ICollection<UserResourceDto>> GetUserResourceAsync(string login);
}