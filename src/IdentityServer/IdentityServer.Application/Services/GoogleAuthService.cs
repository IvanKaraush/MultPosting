using System.Text.Json;
using Haley.Models;
using IdentityServer.Application.Dto;
using IdentityServer.Application.Helpers;
using IdentityServer.Application.Interfaces;
using IdentityServer.Application.Options;
using IdentityServer.Application.Primitives;
using IdentityServer.Domain.Entities;
using IdentityServer.Infrastructure.Interfaces;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;
using Share.Application.Enums;
using Share.Application.Exceptions;
using Share.Application.Options;
using Shared.Infrastructure.Interfaces;

namespace IdentityServer.Application.Services;

public class GoogleAuthService : BaseAuthService, IAuthService
{
    private readonly GoogleOptions _googleOptions;
    private readonly MultiPostingOptions _multiPostingOptions;
    private readonly IHttpProvider _httpProvider;
    private readonly IAccessTokenRepository _accessTokenRepository;

    public GoogleAuthService(UserManager<IdentityUser> userManager, IOptions<JwtOptions> jwtOptions,
        IHttpProvider httpProvider, IAccessTokenRepository accessTokenRepository,
        IOptions<GoogleOptions> googleOptions, IOptions<MultiPostingOptions> multiPostingOptions) : base(userManager, jwtOptions)
    {
        _httpProvider = httpProvider;
        _accessTokenRepository = accessTokenRepository;
        _multiPostingOptions = multiPostingOptions.Value;
        _googleOptions = googleOptions.Value;
    }

    public Task<string> SignUpAsync(AuthRequest request, CancellationToken cancellationToken)
    {
        return Task.FromResult(GetAuthorizationUrl());
    }

    public Task<string> SignInAsync()
    {
        throw new NotImplementedException();
    }

    public async Task<string> SignUpRedirectAsync(AuthRequest request, CancellationToken cancellationToken)
    {
        var tokenResponse = await GenerateJwtTokenAsync(request.Code);

        var headers = new Dictionary<string, string>
        {
            { "Authorization", $"Bearer {tokenResponse.AccessToken}" }
        };

        var userInfoResponse = await _httpProvider.SendGetAsync<GoogleUserInfoResponse>(
            _googleOptions.UserInfoUrl, CancellationToken.None, headers, new JsonSerializerOptions
            {
                PropertyNamingPolicy = JsonNamingPolicy.SnakeCaseLower
            });
        if (userInfoResponse == null)
        {
            throw new BusinessLogicException(ExceptionMessages.ErrorWhileGettingEmailAddress);
        }

        // todo: Use transaction (UoW)
        var jwt = await CreateUserAsync(userInfoResponse.Email, PasswordGenerator.Generate(), cancellationToken);
        var accessToken = new AccessToken(Guid.NewGuid(), userInfoResponse.Email, tokenResponse.AccessToken,
            tokenResponse.RefreshToken);
        await _accessTokenRepository.AddAsync(accessToken, cancellationToken);
        await _accessTokenRepository.SaveChangesAsync(cancellationToken);
        return string.Format(_multiPostingOptions.RedirectContent, $"{_multiPostingOptions.RedirectUrl}?jwt={jwt}");
    }

    private string GetAuthorizationUrl()
    {
        var paramlist = new List<QueryParam>
        {
            new("client_id", _googleOptions.ClientId),
            new("redirect_uri", _googleOptions.RedirectUri),
            new("response_type", "code"),
            new("scope", "email profile https://www.googleapis.com/auth/youtube"),
            new("access_type", "offline"),
            new("prompt", "consent"),
            new("state", nameof(SocialMedia.Google))
        };
        return _googleOptions.AuthorizationUrl + "?" + new QueryParamList(paramlist).GetConcatenatedString();
    }

    private async Task<TokenResponse> GenerateJwtTokenAsync(string code)
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
            await _httpProvider.SendPostAsync<TokenResponse>(_googleOptions.TokenUrl, content,
                CancellationToken.None, options: new JsonSerializerOptions
                {
                    PropertyNamingPolicy = JsonNamingPolicy.SnakeCaseLower
                });
        if (tokenResponse == null)
        {
            throw new BusinessLogicException(ExceptionMessages.ErrorWhileGetAccessToken);
        }

        return tokenResponse;
    }
}