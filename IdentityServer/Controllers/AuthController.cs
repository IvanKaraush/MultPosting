using Haley.Models;
using Haley.Utils;
using Microsoft.AspNetCore.Mvc;

namespace IdentityServer.Controllers;

[ApiController]
public class AuthController : ControllerBase
{
    static string _clientID = @"";

    static string clientSecret = @"";
    static string _authURL = @"https://accounts.google.com/o/oauth2/v2/auth";
    static string _tokenURL = @"https://oauth2.googleapis.com/token";

    [HttpGet("sign-up")]
    public async Task<IActionResult> Test(string code)
    {
        List<QueryParam> paramlist = new List<QueryParam>();
        paramlist.Add(new QueryParam("client_id", _clientID));
        paramlist.Add(new QueryParam("client_secret", clientSecret));
        paramlist.Add(new QueryParam("redirect_uri", @"https://localhost:7182/sign-up"));
        paramlist.Add(new QueryParam("grant_type", "authorization_code"));
        paramlist.Add(new QueryParam("code", code));
        
        var raw_response = await new FluentClient()
                .WithEndPoint(_tokenURL)
                .WithParameter(new FormEncodedRequest(paramlist))
                .PostAsync();
        var asStringResponseAsync = await raw_response.AsStringResponseAsync();
        var test = "Test";
        return Ok();
    }

    [HttpGet("Test")]
    public IActionResult GetRequestUrl()
    {
        List<QueryParam> paramlist = new List<QueryParam>();
        paramlist.Add(new QueryParam("client_id", _clientID));
        paramlist.Add(new QueryParam("redirect_uri", @"https://localhost:7182/sign-up"));
        paramlist.Add(new QueryParam("response_type", "code"));
        paramlist.Add(new QueryParam("scope", "email profile"));
        return Ok(_authURL + "?" + new QueryParamList(paramlist).GetConcatenatedString());
    }
}