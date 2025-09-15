using Microsoft.AspNetCore.Mvc;
using MultiPosting.Application.Dto;
using MultiPosting.Application.Interfaces;

namespace MultiPosting.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class MultiPostController : ControllerBase
{
    private readonly ISocialMediaFactory _socialMediaFactory;

    public MultiPostController(ISocialMediaFactory socialMediaFactory)
    {
        _socialMediaFactory = socialMediaFactory;
    }

    [HttpGet]
    public async Task<IActionResult> GetYouTubeChannels([FromQuery] GetUserInfoRequest request)
    {
        var socialMediaService = _socialMediaFactory.GetSocialMediaService(request.SocialMedia);
        var userResources = await socialMediaService.GetUserResourceAsync(request.Login);
        return Ok(userResources);
    }
}