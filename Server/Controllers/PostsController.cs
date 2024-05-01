using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Server.Repositories;

namespace Server.Controllers;

[Authorize]
[Route("[controller]")]
public class PostsController : Controller
{
    private readonly PostsRepository _postsRepository;

    public PostsController(PostsRepository postsRepository) 
    {
        _postsRepository = postsRepository;
    }

    [HttpGet]
    [Route("GetPosts/{page}")]
    public async Task<IActionResult> GetPosts([FromRoute] int page)
    {
        var userId = HttpContext.User.FindFirst(u => u.Type.Contains("nameid"))!.Value;
        var posts = await _postsRepository.GetPosts(Convert.ToInt32(userId), false, page);
        return Ok(posts);
    }

    [HttpGet]
    [Route("GetUserPosts/{id}/{page}")]
    public async Task<IActionResult> GetUserPosts([FromRoute] int id, int page)
    {
        if (id < 0)
            return BadRequest("Id must be greater than 0");

        var userId = HttpContext.User.FindFirst(u => u.Type.Contains("nameid"))!.Value;
        var posts = await _postsRepository.GetUserPostsAsync(id, Convert.ToInt32(userId), page);
        return Ok(posts);
    }

    [HttpGet]
    [Route("Explore/{page}")]
    public async Task<IActionResult> Explore([FromRoute] int page)
    {
        var userId = HttpContext.User.FindFirst(u => u.Type.Contains("nameid"))!.Value;
        var posts = await _postsRepository.GetPosts(Convert.ToInt32(userId), true, page);
        return Ok(posts);
    }

    [HttpPost]
    [Route("CreatePost")]
    public async Task<IActionResult> CreatePost([FromBody] string content)
    {
        if (content.Length == 0)
            return BadRequest("One or more fields are invalid");

        var userId = HttpContext.User.FindFirst(u => u.Type.Contains("nameid"))!.Value;
        await _postsRepository.CreatePost(content, Convert.ToInt32(userId));
        return Ok();
    }

    [HttpPost]
    [Route("CreatePostWithMedia")]
    public async Task<IActionResult> CreatePostWithMedia(List<IFormFile> files)
    {
        var userId = HttpContext.User.FindFirst(u => u.Type.Contains("nameid"))!.Value;
        var result = await _postsRepository.CreatePostWithMedia(files, Convert.ToInt32(userId));
        return Ok(result);
    }

    [HttpGet]
    [Route("GetMedia/{id}/{page}")]
    public async Task<IActionResult> GetMedia([FromRoute] int id, int page)
    {
        var media = await _postsRepository.GetMediaFromUser(id, page);
        return Ok(media);
    }
}