using System.Text.Json;
using IdentityServer.Application.Dto;
using IdentityServer.Application.Interfaces;
using IdentityServer.Application.Options;
using IdentityServer.Application.Primitives;
using IdentityServer.Domain.Entities;
using IdentityServer.Infrastructure.Interfaces;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;
using Shared.Application.Exceptions;
using Shared.Application.Options;
using Shared.Domain.Enums;
using Shared.Infrastructure.Interfaces;

namespace IdentityServer.Application.Services;

public class TikTokAuthService : BaseAuthService, IAuthService
{
    private readonly TikTokOptions _tikTokOptions;
    private readonly MultiPostingOptions _multiPostingOptions;
    private readonly IHttpProvider _httpProvider;
    private readonly IAccessTokenRepository _accessTokenRepository;

    public TikTokAuthService(UserManager<IdentityUser> userManager, IOptions<JwtOptions> jwtOptions,
        IOptions<TikTokOptions> tikTokOptions, IHttpProvider httpProvider, IAccessTokenRepository accessTokenRepository,
        IOptions<MultiPostingOptions> multiPostingOptions) : base(userManager,
        jwtOptions)
    {
        _httpProvider = httpProvider;
        _accessTokenRepository = accessTokenRepository;
        _multiPostingOptions = multiPostingOptions.Value;
        _tikTokOptions = tikTokOptions.Value;
    }

    public Task<string> SignUpAsync(AuthRequest request, CancellationToken cancellationToken)
    {
        return Task.FromResult(GenerateAuthUrl());
    }

    public Task<string> SignInAsync()
    {
        throw new NotImplementedException();
    }

    public async Task<string> SignUpRedirectAsync(AuthRequest request, CancellationToken cancellationToken)
    {
        var jsonOptions = new JsonSerializerOptions
        {
            PropertyNamingPolicy = JsonNamingPolicy.SnakeCaseLower
        };
        var tokenRequestForm = new Dictionary<string, string>
        {
            ["client_key"] = _tikTokOptions.ClientKey,
            ["client_secret"] = _tikTokOptions.ClientSecret,
            ["code"] = request.Code,
            ["grant_type"] = "authorization_code",
            ["redirect_uri"] = _tikTokOptions.RedirectUri
        };

        var tokenResponse = await _httpProvider.SendPostAsync<TokenResponse>(
            _tikTokOptions.TokenUrl,
            new FormUrlEncodedContent(tokenRequestForm),
            cancellationToken,
            options: jsonOptions) ?? throw new BusinessLogicException(ExceptionMessages.ErrorWhileGetAccessToken);

        var headers = new Dictionary<string, string>
        {
            ["Authorization"] = $"Bearer {tokenResponse.AccessToken}"
        };

        var response = await _httpProvider.SendGetAsync<TikTokUserInfoResponse>(
            $"{_tikTokOptions.UserInfoUrl}?fields={Uri.EscapeDataString("open_id")}", cancellationToken,
            headers, jsonOptions) ?? throw new BusinessLogicException(ExceptionMessages.ErrorWhileGetUserInfo);

        var accessToken = new AccessToken(Guid.NewGuid(), response.Data.User.OpenId, tokenResponse.AccessToken,
            tokenResponse.RefreshToken);
        await _accessTokenRepository.AddAsync(accessToken, cancellationToken);
        await _accessTokenRepository.SaveChangesAsync(cancellationToken);
        return string.Format(_multiPostingOptions.RedirectContent,
            $"{_tikTokOptions.RedirectApplicationUrl}?id={response.Data.User.OpenId}&social_media={SocialMedia.TikTok}");
    }

    private string GenerateAuthUrl()
    {
        return
            $"{_tikTokOptions.AuthorizationUrl}?client_key={_tikTokOptions.ClientKey}&redirect_uri={_tikTokOptions.RedirectUri}&scope=user.info.basic&response_type=code&state={nameof(SocialMedia.TikTok)}";
    }
}