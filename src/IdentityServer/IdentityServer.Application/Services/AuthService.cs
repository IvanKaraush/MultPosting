using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using IdentityServer.Application.Exceptions;
using IdentityServer.Application.Interfaces;
using IdentityServer.Application.Options;
using IdentityServer.Application.Primitives;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using JwtRegisteredClaimNames = Microsoft.IdentityModel.JsonWebTokens.JwtRegisteredClaimNames;

namespace IdentityServer.Application.Services;

public class AuthService : IAuthService
{
    private readonly UserManager<IdentityUser> _userManager;
    private readonly JwtOptions _jwtOptions;

    public AuthService(UserManager<IdentityUser> userManager, IOptions<JwtOptions> jwtOptions)
    {
        _userManager = userManager;
        _jwtOptions = jwtOptions.Value;
    }

    public async Task<string> CreateUserAsync(string email, string password)
    {
        var user = await _userManager.FindByEmailAsync(email);
        if (user != null)
        {
            throw new UserAlreadyExistException(string.Format(ExceptionMessages.UserAlreadyExistMessage, email));
        }

        var newUser = new IdentityUser(email);
        var result = await _userManager.CreateAsync(newUser, password);
        if (!result.Succeeded)
        {
            throw new Exception(); // todo: Handle exception
        }
        return GenerateJwt(newUser);
    }

    public async Task<string> GenerateJwtTokenAsync(string email, string password)
    {
        var user = await _userManager.FindByNameAsync(email);
        if (user == null || !await _userManager.CheckPasswordAsync(user, password))
        {
            return string.Empty; // todo: Throw exception
        }

        return GenerateJwt(user);
    }

    private string GenerateJwt(IdentityUser user)
    {
        var claims = new[]
        {
            new Claim(JwtRegisteredClaimNames.Sub, user.Id),
            new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
            new Claim(ClaimTypes.Name, user.UserName)
        };
        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwtOptions.SecretKey));
        var credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
        var token = new JwtSecurityToken(issuer: _jwtOptions.ValidIssuer, audience: _jwtOptions.ValidAudience,
            claims: claims, signingCredentials: credentials);

        return new JwtSecurityTokenHandler().WriteToken(token);
    }
}