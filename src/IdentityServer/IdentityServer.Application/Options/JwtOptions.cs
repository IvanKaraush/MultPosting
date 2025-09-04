namespace IdentityServer.Application.Options;

public class JwtOptions
{
    public required string ValidIssuer { get; init; }
    public required string ValidAudience { get; init; }
    public required string SecretKey { get; init; }
}