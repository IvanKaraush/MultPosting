using IdentityServer.Application.Dto;
using IdentityServer.Application.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Shared.Application.Enums;

namespace IdentityServer.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class AuthController : ControllerBase
{
    private readonly IAuthServiceFactory _authServiceFactory;

    public AuthController(IAuthServiceFactory authServiceFactory)
    {
        _authServiceFactory = authServiceFactory;
    }

    [HttpPost("sign-in")]
    public async Task<IActionResult> SignIn([FromBody] CreateUserRequest request)
    {
        var authService = _authServiceFactory.GetAuthService(SocialMedia.None);

        var jwt = await authService.SignInAsync();
        return Ok(jwt);
    }

    [HttpPost("sign-up")]
    public async Task<IActionResult> SignUp([FromBody] AuthRequest request, CancellationToken cancellationToken)
    {
        var authService = _authServiceFactory.GetAuthService(request.State);
        var result = await authService.SignUpAsync(request, cancellationToken);
        return Ok(result);
    }

    [HttpGet("sign-up-redirect")]
    public async Task<IActionResult> SignUpRedirect([FromQuery] AuthRequest request,
        CancellationToken cancellationToken)
    {
        var authService = _authServiceFactory.GetAuthService(request.State);

        var content = await authService.SignUpRedirectAsync(request, cancellationToken);
        return Content(content, "text/html; charset=utf-8");
    }
}