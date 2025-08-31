using Haley.Models;
using IdentityServer.Api.Options;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;

namespace IdentityServer.Api.Controllers;

[ApiController]
public class AuthController : ControllerBase
{
    private readonly GoogleOptions _googleOptions;

    public AuthController(IOptions<GoogleOptions> googleOptions)
    {
        _googleOptions = googleOptions.Value;
    }

    [HttpGet("sign-up")]
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

        using (var client = new HttpClient())
        {
            var response = await client.PostAsync(_googleOptions.TokenUrl, content);

            response.EnsureSuccessStatusCode();

            return Ok();
        }
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