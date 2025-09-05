namespace IdentityServer.Application.Dto;

public class CreateUserRequest
{
    public string Email { get; init; }
    public string Password { get; init; }
}