using System.Security.Cryptography;
using System.Text;
using System.Text.Json;
using IdentityServer.Application.Dto;
using IdentityServer.Application.Interfaces;
using IdentityServer.Application.Primitives;
using IdentityServer.Domain.Entities;
using IdentityServer.Infrastructure.Interfaces;
using Microsoft.Extensions.Options;
using Shared.Application.Exceptions;
using Shared.Application.Options;
using Shared.Domain.Enums;
using Shared.Infrastructure.Interfaces;

namespace IdentityServer.Application.Services;

public class VkAuthService : IAuthService
{
    private readonly VkOptions _vkOptions;
    private readonly MultiPostingOptions _multiPostingOptions;
    private static string _codeVerifier = string.Empty;
    private readonly IHttpProvider _httpProvider;
    private readonly IAccessTokenRepository _accessTokenRepository;

    public VkAuthService(IOptions<VkOptions> vkOptions, IHttpProvider httpProvider,
        IAccessTokenRepository accessTokenRepository, IOptions<MultiPostingOptions> multiPostingOptions)
    {
        _httpProvider = httpProvider;
        _accessTokenRepository = accessTokenRepository;
        _multiPostingOptions = multiPostingOptions.Value;
        _vkOptions = vkOptions.Value;
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
        var formParams = new Dictionary<string, string>
        {
            ["grant_type"] = "authorization_code",
            ["client_id"] = _vkOptions.ApplicationId.ToString(),
            ["redirect_uri"] = _vkOptions.RedirectUrl,
            ["code"] = request.Code,
            ["code_verifier"] = _codeVerifier,
            ["device_id"] = request.DeviceId,
        };

        var content = new FormUrlEncodedContent(formParams);
        var tokenResponse = await _httpProvider.SendPostAsync<TokenResponse>(_vkOptions.TokenUrl, content,
            cancellationToken, options: new JsonSerializerOptions
            {
                PropertyNamingPolicy = JsonNamingPolicy.SnakeCaseLower
            }) ?? throw new BusinessLogicException(ExceptionMessages.ErrorWhileGetAccessToken);

        var userInfo = await _httpProvider.SendGetAsync<UserInfoResponse>(
            $"{_vkOptions.UserInfoUrl}?access_token={tokenResponse.AccessToken}&v={_vkOptions.Version}",
            cancellationToken, options: new JsonSerializerOptions
            {
                PropertyNamingPolicy = JsonNamingPolicy.SnakeCaseLower
            }) ?? throw new BusinessLogicException(ExceptionMessages.ErrorWhileGetUserInfo);
        var phone = userInfo.Response.Phone[^2..];
        var accessToken =
            new AccessToken(Guid.NewGuid(), phone, tokenResponse.AccessToken, tokenResponse.RefreshToken);
        await _accessTokenRepository.AddAsync(accessToken, cancellationToken);
        await _accessTokenRepository.SaveChangesAsync(cancellationToken);
        return string.Format(_multiPostingOptions.RedirectContent, $"{_vkOptions.RedirectApplicationUrl}?id={phone}&social_media={SocialMedia.Vk}");
    }

    private string GetAuthorizationUrl()
    {
        using (var rng = RandomNumberGenerator.Create())
        {
            var randomBytes = new byte[64];
            rng.GetBytes(randomBytes);
            var codeVerifier = Base64UrlEncode(randomBytes);
            _codeVerifier = codeVerifier;
            var hash = SHA256.HashData(Encoding.ASCII.GetBytes(codeVerifier));
            var codeChallenge = Base64UrlEncode(hash);

            var authorizeUrl = $"https://id.vk.ru/authorize?" +
                               $"response_type=code&client_id={_vkOptions.ApplicationId}" +
                               $"&redirect_uri={Uri.EscapeDataString(_vkOptions.RedirectUrl)}" +
                               $"&code_challenge={codeChallenge}" +
                               $"&code_challenge_method=S256&state={nameof(SocialMedia.Vk)}&scope=email";
            return authorizeUrl;
        }
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