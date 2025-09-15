namespace IdentityServer.Application.Dto;

public class TikTokUserInfoResponse
{
    public TikTokUserInfoData Data { get; set; }
    public TikTokError Error { get; set; }
}

public class TikTokUserInfoData
{
    public TikTokUser User { get; set; }
}

public class TikTokUser
{
    public string DisplayName { get; set; }
    public string OpenId { get; set; }
}

public class TikTokError
{
    public string Code { get; set; }
    public string Message { get; set; }
    public string LogId { get; set; }
}