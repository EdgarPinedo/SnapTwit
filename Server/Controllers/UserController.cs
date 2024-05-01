using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Server.Repositories;

namespace Server.Controllers;

[Authorize]
[Route("[controller]")]
public class UserController : Controller
{
    private readonly UserRepository _userRepository;

    public UserController(UserRepository userRepository)
    {
        _userRepository = userRepository;
    }

    [HttpGet]
    [Route("GetRandom")]
    public async Task<IActionResult> GetRandom()
    {
        var userId = HttpContext.User.FindFirst(u => u.Type.Contains("nameid"))!.Value;
        var randomUsers = await _userRepository.GetRandomAsync(Convert.ToInt32(userId));
        return Ok(randomUsers);
    }

    [HttpGet]
    [Route("GetProfile/{id}")]
    public async Task<IActionResult> GetProfile([FromRoute] int id)
    {
        var userId = HttpContext.User.FindFirst(u => u.Type.Contains("nameid"))!.Value;
        var profile = await _userRepository.GetProfile(id, Convert.ToInt32(userId));

        if (profile is null)
            return NotFound("Profile not found");

        return Ok(profile);
    }

    [HttpGet]
    [Route("Search/{query}")]
    public async Task<IActionResult> Search([FromRoute] string query)
    {
        var searchResults = await _userRepository.SearchUser(query);
        return Ok(searchResults);
    }

    [HttpPost]
    [Route("UpdateProfilePicture")]
    public async Task<IActionResult> UpdateProfilePicture(List<IFormFile> files)
    {
        var userId = HttpContext.User.FindFirst(u => u.Type.Contains("nameid"))!.Value;
        var result = await _userRepository.UpdateProfilePicture(files, Convert.ToInt32(userId));

        return Ok(result);
    }

    [HttpPost]
    [Route("UpdateCoverPicture")]
    public async Task<IActionResult> UpdateCoverPicture(List<IFormFile> files)
    {
        var userId = HttpContext.User.FindFirst(u => u.Type.Contains("nameid"))!.Value;
        var result = await _userRepository.UpdateCoverPicture(files, Convert.ToInt32(userId));

        return Ok(result);
    }

    [HttpPost]
    [Route("FollowUser/{id}")]
    public async Task<IActionResult> FollowUser([FromRoute] int id)
    {
        var userId = HttpContext.User.FindFirst(u => u.Type.Contains("nameid"))!.Value;
        await _userRepository.FollowUser(Convert.ToInt32(userId), id);

        return Ok();
    }

    [HttpPost]
    [Route("UnfollowUser/{id}")]
    public async Task<IActionResult> UnfollowUser([FromRoute] int id)
    {
        var userId = HttpContext.User.FindFirst(u => u.Type.Contains("nameid"))!.Value;
        await _userRepository.UnfollowUser(Convert.ToInt32(userId), id);

        return Ok();
    }
}