using Shared.Domain.Entities;

namespace IdentityServer.Domain.Entities;

public class AccessToken : BaseEntity
{
    public string Email { get; private set; }
    public string Token { get; private set; }

    public string RefreshToken { get; private set; }

    public AccessToken(Guid id, string email, string token, string refreshToken) : base(id)
    {
        Email = email;
        Token = token;
        RefreshToken = refreshToken;
    }
}