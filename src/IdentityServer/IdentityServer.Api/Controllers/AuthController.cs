using System.Net.Http.Headers;
using System.Text.Json;
using Haley.Models;
using IdentityServer.Application.Dto;
using IdentityServer.Application.Interfaces;
using IdentityServer.Application.Options;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;

namespace IdentityServer.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class AuthController : ControllerBase
{
    private readonly GoogleOptions _googleOptions;
    private readonly IAuthService _authService;
    private const string RedirectUri = "multiposting://auth/callback";

    public AuthController(IOptions<GoogleOptions> googleOptions, IAuthService authService)
    {
        _authService = authService;
        _googleOptions = googleOptions.Value;
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
        var formParams = new Dictionary<string, string>
        {
            ["client_id"] = _googleOptions.ClientId,
            ["client_secret"] = _googleOptions.ClientSecret,
            ["redirect_uri"] = _googleOptions.RedirectUri,
            ["grant_type"] = "authorization_code",
            ["code"] = code
        };

        var content = new FormUrlEncodedContent(formParams);
        ResponseDto? responseDto = null;

        using (var client = new HttpClient())
        {
            var response = await client.PostAsync(_googleOptions.TokenUrl, content);

            var responseString = await response.Content.ReadAsStringAsync();
            responseDto = JsonSerializer.Deserialize<ResponseDto>(responseString);
        }

        var email = string.Empty;
        using (var client = new HttpClient())
        {
            client.DefaultRequestHeaders.Authorization =
                new AuthenticationHeaderValue("Bearer", responseDto.access_token);
            var response = await client.GetAsync("https://www.googleapis.com/oauth2/v3/userinfo");
            response.EnsureSuccessStatusCode();
            var userInfo = await response.Content.ReadAsStringAsync();
            var user = JsonSerializer.Deserialize<Dictionary<string, object>>(userInfo);
            email = ((JsonElement)user["email"]).GetRawText();
        }

        var jwt = await _authService.CreateUserAsync(email, "TestPassword123@#%^");

        return Content(
            $"<!DOCTYPE html>\n<html>\n  <head>\n    <meta http-equiv=\"refresh\" content=\"0;url={RedirectUri}?jwt={jwt}\" />\n    <script>\n      window.location = \"{RedirectUri}?jwt={jwt}\";\n    </script>\n  </head>\n  <body>\n    Redirecting…\n  </body>\n</html>",
            "text/html");
    }

    [HttpGet("google-sign-up")]
    public IActionResult SignUpWithGoogle()
    {
        var paramlist = new List<QueryParam>
        {
            new("client_id", _googleOptions.ClientId),
            new("redirect_uri", _googleOptions.RedirectUri),
            new("response_type", "code"),
            new("scope", "email profile")
        };
        return Ok(_googleOptions.AuthorizationUrl + "?" + new QueryParamList(paramlist).GetConcatenatedString());
    }
}