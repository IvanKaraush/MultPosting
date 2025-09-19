using Haley.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using MultiPosting.Application.Dto;
using MultiPosting.Application.Interfaces;
using Shared.Application.Options;
using Shared.Domain.Enums;

namespace MultiPosting.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class MultiPostController : ControllerBase
{
    private readonly ISocialMediaFactory _socialMediaFactory;
    private readonly GoogleOptions _googleOptions;

    public MultiPostController(ISocialMediaFactory socialMediaFactory, IOptions<GoogleOptions> googleOptions)
    {
        _socialMediaFactory = socialMediaFactory;
        _googleOptions = googleOptions.Value;
    }

    [HttpGet]
    public async Task<IActionResult> GetYouTubeChannels([FromQuery] GetUserInfoRequest request)
    {
        var socialMediaService = _socialMediaFactory.GetSocialMediaService(request.SocialMedia);
        var userResources = await socialMediaService.GetUserResourceAsync(request.Login);
        return Ok(userResources);
    }
    
    [HttpGet("you-tube")]
    public IActionResult GetYouTubeChannelUrl()
    {
        var paramlist = new List<QueryParam>
        {
            new("client_id", _googleOptions.ClientId),
            new("redirect_uri", _googleOptions.RedirectUri),
            new("response_type", "code"),
            new("scope", "email profile https://www.googleapis.com/auth/youtube"),
            new("access_type", "offline"),
            new("prompt", "consent"),
            new("state", nameof(SocialMedia.YouTube))
        };
        return Ok(_googleOptions.AuthorizationUrl + "?" + new QueryParamList(paramlist).GetConcatenatedString());
    }
}