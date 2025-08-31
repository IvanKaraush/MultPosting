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
    public async Task<IActionResult> AddAccountYoutube()
    {
        await _youtubeService.AddAccountAsync();
        return Ok();
    }
}