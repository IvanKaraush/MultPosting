using Microsoft.AspNetCore.Mvc;
using Shared.Domain.Enums;

namespace IdentityServer.Application.Dto;

public class AuthRequest
{
    [FromQuery(Name = "code")] 
    public string? Code { get; init; }
    public string? Email { get; init; }
    public string? Password { get; init; }
    [FromQuery(Name = "device_id")] 
    public string? DeviceId { get; init; }
    public SocialMedia State { get; init; }
}