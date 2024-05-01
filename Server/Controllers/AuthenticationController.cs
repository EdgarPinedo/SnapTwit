using Microsoft.AspNetCore.Mvc;
using Server.Authentication;
using Snaptwit.Shared.DTOs;

namespace Server.Controllers;

[Route("[controller]")]
public class AuthenticationController : Controller
{
    private readonly AccountService _accountService;
    
    public AuthenticationController(AccountService accountService) 
        => _accountService = accountService;


    [HttpPost]
    [Route("Login")]
    public async Task<IActionResult> Login([FromBody] LoginRequest request)
    {
        var user = await _accountService.GetUserAsync(request);

        if (user is null)
        {
            return NotFound("Your email and/or password are not correct");
        }
        
        var loginResponse = _accountService.GenerateLoginResponse(user.Id, user.Name);
        return Ok(loginResponse);
    }

    [HttpPost]
    [Route("Register")]
    public async Task<IActionResult> Register([FromBody] RegisterRequest request)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest("One or more fields are invalid");
        }

        if (await _accountService.EmailAlreadyExistsAsync(request.Email))
        {
            return BadRequest("Email is already registered");
        }

        var user = await _accountService.RegisterUserAsync(request);
        var loginResponse = _accountService.GenerateLoginResponse(user.Id, user.Name);
        return Ok(loginResponse);
    }
}