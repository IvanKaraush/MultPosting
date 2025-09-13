using System.Security.Cryptography;
using System.Text;
using System.Text.Json;
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

    [HttpGet("vk")]
    public async Task<IActionResult> SignUpWithVk(string code)
    {
        if (string.IsNullOrEmpty(code)) return BadRequest("Missing code");

        var clientId = 0;
        var clientSecret = "";
        var redirectUri = "https://4053388a52e1.ngrok-free.app/api/MultiPost/vk";

        using var http = new HttpClient();
        var tokenUrl =
            $"https://oauth.vk.com/access_token?client_id={clientId}&client_secret={clientSecret}&redirect_uri={Uri.EscapeDataString(redirectUri)}&code={code}";
        var response = await http.GetAsync(tokenUrl);
        if (!response.IsSuccessStatusCode) return StatusCode((int)response.StatusCode, "Token request failed");

        var json = await response.Content.ReadFromJsonAsync<JsonElement>();
        if (!json.TryGetProperty("access_token", out var tokenProp)) return BadRequest("No access_token in response");

        var accessToken = tokenProp.GetString();
        var userId = json.GetProperty("user_id").GetInt32();

        var jwt = await _authService.GenerateJwtTokenAsync("", "");
        return Content(
            $"<!DOCTYPE html>\n<html>\n  <head>\n    <meta http-equiv=\"refresh\" content=\"0;url={RedirectUri}?jwt={jwt}\" />\n    <script>\n      window.location = \"{RedirectUri}?jwt={jwt}\";\n    </script>\n  </head>\n  <body>\n    Redirecting…\n  </body>\n</html>",
            "text/html");
    }

    [HttpGet("vk-sign-up")]
    public IActionResult SignUpWithVk()
    {
        using var rng = RandomNumberGenerator.Create();
        var randomBytes = new byte[64];
        rng.GetBytes(randomBytes);
        var codeVerifier = Base64UrlEncode(randomBytes);

        using var sha256 = SHA256.Create();
        var hash = sha256.ComputeHash(Encoding.ASCII.GetBytes(codeVerifier));
        var codeChallenge = Base64UrlEncode(hash);

        var authorizeUrl = $"https://id.vk.com/authorize?" +
                           $"response_type=code&client_id={54102468}" +
                           $"&redirect_uri={Uri.EscapeDataString("https://4053388a52e1.ngrok-free.app/api/MultiPost/vk")}" +
                           $"&code_challenge={codeChallenge}" +
                           $"&code_challenge_method=S256&state=12345";
        return Ok(authorizeUrl);
    }

    private static string Base64UrlEncode(byte[] bytes)
    {
        var base64 = Convert.ToBase64String(bytes);
        return base64
            .Replace("+", "-")
            .Replace("/", "_")
            .TrimEnd('=');
    }
}