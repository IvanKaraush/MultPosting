using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Haley.Models;
using IdentityServer.Application.Dto;
using IdentityServer.Application.Exceptions;
using IdentityServer.Application.Extensions;
using IdentityServer.Application.Helpers;
using IdentityServer.Application.Interfaces;
using IdentityServer.Application.Options;
using IdentityServer.Application.Primitives;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using Share.Application.Exceptions;
using Shared.Infrastructure.Interfaces;
using JwtRegisteredClaimNames = Microsoft.IdentityModel.JsonWebTokens.JwtRegisteredClaimNames;

namespace IdentityServer.Application.Services;

public class AuthService : IAuthService
{
    private readonly UserManager<IdentityUser> _userManager;
    private readonly JwtOptions _jwtOptions;
    private readonly GoogleOptions _googleOptions;
    private readonly IHttpProvider _httpProvider;

    public AuthService(UserManager<IdentityUser> userManager, IOptions<JwtOptions> jwtOptions,
        IOptions<GoogleOptions> googleOptions, IHttpProvider httpProvider)
    {
        _userManager = userManager;
        _httpProvider = httpProvider;
        _jwtOptions = jwtOptions.Value;
        _googleOptions = googleOptions.Value;
    }

    public async Task<string> CreateUserAsync(string email, string password)
    {
        var user = await _userManager.FindByEmailAsync(email);
        if (user != null)
        {
            throw new UserAlreadyExistException(string.Format(ExceptionMessages.UserAlreadyExist, email));
        }

        var newUser = new IdentityUser(email);
        var result = await _userManager.CreateAsync(newUser, password);
        if (!result.Succeeded)
        {
            throw new Exception(string.Join(", ", result.Errors)); // todo: Handle exception
        }

        return GenerateJwt(newUser);
    }

    public async Task<string> GenerateJwtTokenAsync(string email, string password)
    {
        var user = await _userManager.FindByEmailAsync(email);
        if (user == null || !await _userManager.CheckPasswordAsync(user, password))
        {
            throw new InvalidCredentialsException(ExceptionMessages.InvalidCredentials);
        }

        return GenerateJwt(user);
    }

    public async Task<string> GoogleSignUpAsync(string code)
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
        var tokenResponse =
            await _httpProvider.SendPostAsync<FormUrlEncodedContent, TokenResponse>(_googleOptions.TokenUrl, content,
                CancellationToken.None);

        var headers = new Dictionary<string, string>
        {
            { "Authoriation", $"Bearer {tokenResponse?.AccessToken}" }
        };
        var userInfoResponse = await _httpProvider.SendPostAsync<FormUrlEncodedContent, GoogleUserInfoResponse>(
            _googleOptions.TokenUrl, content,
            CancellationToken.None, headers);
        if (userInfoResponse == null)
        {
            throw new BusinessLogicException(ExceptionMessages.ErrorWhileGettingEmailAddress);
        }

        return await CreateUserAsync(userInfoResponse.Email, PasswordGenerator.Generate());
    }

    public string GenerateAuthorizationUrl()
    {
        var paramlist = new List<QueryParam>
        {
            new("client_id", _googleOptions.ClientId),
            new("redirect_uri", _googleOptions.RedirectUri),
            new("response_type", "code"),
            new("scope", "email profile")
        };
        return _googleOptions.AuthorizationUrl + "?" + new QueryParamList(paramlist).GetConcatenatedString();
    }

    private string GenerateJwt(IdentityUser user)
    {
        ArgumentNullException.ThrowIfNull(user);
        var claims = new[]
        {
            new Claim(JwtRegisteredClaimNames.Sub, user.Id),
            new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
            new Claim(ClaimTypes.Name, user.UserName!)
        };
        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwtOptions.SecretKey));
        var credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
        var token = new JwtSecurityToken(issuer: _jwtOptions.ValidIssuer, audience: _jwtOptions.ValidAudience,
            claims: claims, signingCredentials: credentials);

        return new JwtSecurityTokenHandler().WriteToken(token);
    }
}