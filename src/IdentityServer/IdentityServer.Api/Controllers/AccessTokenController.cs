using IdentityServer.Application.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace IdentityServer.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class AccessTokenController : ControllerBase
{
    private readonly IAccessTokenService _accessTokenService;

    public AccessTokenController(IAccessTokenService accessTokenService)
    {
        _accessTokenService = accessTokenService;
    }

    [HttpGet]
    public async Task<IActionResult> GetAccessTokeByEmail([FromQuery] string email)
    {
        var accessToken = await _accessTokenService.GetAccessTokenByEmailAsync(email);
        return Ok(accessToken);
    }
}