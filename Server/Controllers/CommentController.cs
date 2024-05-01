using Microsoft.AspNetCore.Mvc;
using Server.Repositories;
using Snaptwit.Shared.DTOs;

namespace Server.Controllers;

[Route("[controller]")]
public class CommentController : Controller
{
    private readonly CommentRepository _commentRepository;

    public CommentController(CommentRepository commentRepository)
    {
        _commentRepository = commentRepository;
    }

    [HttpGet]
    [Route("GetComments/{postId}/{page}")]
    public async Task<IActionResult> GetComments([FromRoute] int postId, int page)
    {
        var userId = HttpContext.User.FindFirst(u => u.Type.Contains("nameid"))!.Value;
        var comments = await _commentRepository.GetComments(postId, null, Convert.ToInt32(userId), page);
        return Ok(comments);
    }

    [HttpGet]
    [Route("GetReplies/{commentId}/{page}")]
    public async Task<IActionResult> GetReplies([FromRoute] int commentId, int page)
    {
        var userId = HttpContext.User.FindFirst(u => u.Type.Contains("nameid"))!.Value;
        var comments = await _commentRepository.GetComments(null, commentId, Convert.ToInt32(userId), page);
        return Ok(comments);
    }

    [HttpPost]
    [Route("Reply")]
    public async Task<IActionResult> Reply([FromBody] CommentRequest reply)
    {
        if(!ModelState.IsValid)
            return BadRequest("One or more fields are invalid");

        var userId = HttpContext.User.FindFirst(u => u.Type.Contains("nameid"))!.Value;
        await _commentRepository.Reply(reply, Convert.ToInt32(userId));
        return Ok();
    }
}