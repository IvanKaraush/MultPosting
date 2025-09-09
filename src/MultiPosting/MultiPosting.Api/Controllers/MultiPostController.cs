using System.Security.Cryptography;
using System.Text;
using System.Text.Json;
using Microsoft.AspNetCore.Mvc;
using MultiPosting.Application.Interfaces;

namespace MultiPosting.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class MultiPostController : ControllerBase
{
    private readonly IYoutubeService _youtubeService;

    public MultiPostController(IYoutubeService youtubeService)
    {
        _youtubeService = youtubeService;
    }

    [HttpGet]
    public async Task<IActionResult> AddAccountYoutube([FromQuery] string email)
    {
        var youtubeChannels = await _youtubeService.AddAccountAsync(email);
        return Ok(youtubeChannels);
    }

    [HttpGet("Test")]
    public IActionResult Get()
    {
        using var rng = RandomNumberGenerator.Create();
        byte[] randomBytes = new byte[64];
        rng.GetBytes(randomBytes);
        string codeVerifier = Base64UrlEncode(randomBytes); // URL-safe, no padding

// 2. Derive code_challenge using SHA256
        using var sha256 = SHA256.Create();
        byte[] hash = sha256.ComputeHash(Encoding.ASCII.GetBytes(codeVerifier));
        string codeChallenge = Base64UrlEncode(hash);

// 3. Redirect user to VK ID authorization
        var authorizeUrl = $"https://id.vk.com/authorize?" +
                           $"response_type=code&client_id={54102468}" +
                           $"&redirect_uri={Uri.EscapeDataString("https://4053388a52e1.ngrok-free.app/api/MultiPost/vk")}" +
                           $"&code_challenge={codeChallenge}" +
                           $"&code_challenge_method=S256&state=12345";
        return Ok(authorizeUrl);
    }
    
    public static string Base64UrlEncode(byte[] bytes)
    {
        // Преобразуем в стандартную Base64 строку
        string base64 = Convert.ToBase64String(bytes);
        // Заменяем символы + → –, / → _, и удаляем символы = в конце
        return base64
            .Replace("+", "-")
            .Replace("/", "_")
            .TrimEnd('=');
    }
    
    [HttpGet("vk")]
    public async Task<IActionResult> AddAccountVk([FromQuery]string code)
    {
        if (string.IsNullOrEmpty(code)) return BadRequest("Missing code");

        var clientId = 0;
        var clientSecret = "";
        var redirectUri = "https://4053388a52e1.ngrok-free.app/api/MultiPost/vk";

        using var http = new HttpClient();
        var tokenUrl = $"https://oauth.vk.com/access_token?client_id={clientId}&client_secret={clientSecret}&redirect_uri={Uri.EscapeDataString(redirectUri)}&code={code}";
        var response = await http.GetAsync(tokenUrl);
        if (!response.IsSuccessStatusCode) return StatusCode((int)response.StatusCode, "Token request failed");

        var json = await response.Content.ReadFromJsonAsync<JsonElement>();
        if (!json.TryGetProperty("access_token", out var tokenProp)) return BadRequest("No access_token in response");

        var accessToken = tokenProp.GetString();
        var userId = json.GetProperty("user_id").GetInt32();

        // Optional: call VK API for user info
        // string jwt = GenerateJwtToken(userId, accessToken);

        // Redirect to Flutter app via deep link
        var jwt = GenerateJwtToken(userId);
        var redirect = $"myapp://auth?token={jwt}";
        return Redirect(redirect);
    }
    private string GenerateJwtToken(int userId)
    {
        // Implement JWT generation with your secret/claims
        return "YOUR_JWT_TOKEN";
    }
}