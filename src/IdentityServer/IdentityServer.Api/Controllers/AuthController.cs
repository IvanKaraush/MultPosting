using IdentityServer.Application.Dto;
using IdentityServer.Application.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace IdentityServer.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class AuthController : ControllerBase
{
    private readonly IAuthService _authService;
    private const string RedirectUri = "multiposting://auth/callback";

    public AuthController(IAuthService authService)
    {
        _authService = authService;
    }

    [HttpPost("sign-in")]
    public async Task<IActionResult> SignIn([FromBody] CreateUserRequest request)
    {
        var jwt = await _authService.GenerateJwtTokenAsync(request.Email, request.Password);
        return Ok(jwt);
    }

    [HttpPost("sign-up")]
    public async Task<IActionResult> SignUp([FromBody] CreateUserRequest request)
    {
        var jwt = await _authService.CreateUserAsync(request.Email, request.Password);
        return Ok(jwt);
    }

    [HttpGet("google-sign-up-redirect")]
    public async Task<IActionResult> SignUp(string code)
    {
        var jwt = await _authService.GoogleSignUpAsync(code);
        return Content(
            $"<!DOCTYPE html>\n<html>\n  <head>\n    <meta http-equiv=\"refresh\" content=\"0;url={RedirectUri}?jwt={jwt}\" />\n    <script>\n      window.location = \"{RedirectUri}?jwt={jwt}\";\n    </script>\n  </head>\n  <body>\n    Redirecting…\n  </body>\n</html>",
            "text/html");
    }

    [HttpGet("google-sign-up")]
    public IActionResult SignUpWithGoogle()
    {
        var authorizationUrl = _authService.GenerateAuthorizationUrl();
        return Ok(authorizationUrl);
    }
}