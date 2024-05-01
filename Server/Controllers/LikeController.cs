using Microsoft.AspNetCore.Mvc;
using Server.Repositories;

namespace Server.Controllers;

[Route("[controller]")]
public class LikeController : Controller
{
    private readonly LikeRepository _likeRepository;

    public LikeController(LikeRepository likeRepository)
    {
        _likeRepository = likeRepository;
    }

    [HttpPost]
    [Route("LikePost/{postId}")]
    public async Task<IActionResult> LikePost([FromRoute] int postId)
    {
        var userId = HttpContext.User.FindFirst(u => u.Type.Contains("nameid"))!.Value;
        await _likeRepository.LikePost(postId, Convert.ToInt32(userId));
        return Ok();
    }

    [HttpPost]
    [Route("DislikePost/{postId}")]
    public async Task<IActionResult> DislikePost([FromRoute] int postId)
    {
        var userId = HttpContext.User.FindFirst(u => u.Type.Contains("nameid"))!.Value;
        await _likeRepository.DislikePost(postId, Convert.ToInt32(userId));
        return Ok();
    }

    [HttpPost]
    [Route("LikeComment/{commentId}")]
    public async Task<IActionResult> LikeComment([FromRoute] int commentId)
    {
        var userId = HttpContext.User.FindFirst(u => u.Type.Contains("nameid"))!.Value;
        await _likeRepository.LikeComment(commentId, Convert.ToInt32(userId));
        return Ok();
    }

    [HttpPost]
    [Route("DislikeComment/{commentId}")]
    public async Task<IActionResult> DislikeComment([FromRoute] int commentId)
    {
        var userId = HttpContext.User.FindFirst(u => u.Type.Contains("nameid"))!.Value;
        await _likeRepository.DislikeComment(commentId, Convert.ToInt32(userId));
        return Ok();
    }
}