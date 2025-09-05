namespace IdentityServer.Application.Dto;

public class GoogleUserInfoResponse
{
    public string FullName { get; init; }

    public string GivenName { get; init; }

    public string FamilyName { get; init; }

    public string Email { get; init; }
}