using Microsoft.AspNetCore.Mvc;

namespace IdentityServer.Application.Dto;
public class VkAuthorizationRequest
{
    [FromQuery(Name = "code")]
    public string Code { get; init; }

    [FromQuery(Name = "device_id")]
    public string DeviceId { get; init; }
}
