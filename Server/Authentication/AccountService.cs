using Microsoft.EntityFrameworkCore;
using Server.Data;
using Snaptwit.Shared;
using Snaptwit.Shared.DTOs;

namespace Server.Authentication;

public class AccountService
{
    private readonly AppDbContext _context;
    private readonly IConfiguration _config;

    public AccountService(AppDbContext context, IConfiguration config)
    {
        _context = context;
        _config = config;
    }

    public async Task<User?> GetUserAsync(LoginRequest request) 
        => await _context.Users.FirstOrDefaultAsync(
            u => u.Email == request.Email && u.Password == request.Password);

    public async Task<bool> EmailAlreadyExistsAsync(string email)
    {
        _context.Database.EnsureCreated();
        return await _context.Users.AnyAsync(u => u.Email == email);
    }

    public async Task<User> RegisterUserAsync(RegisterRequest request)
    {
        User user = new()
        {
            Name = request.Name,
            Email = request.Email,
            PhoneNumber = request.PhoneNumber,
            Password = request.Password,
            JoinedDate = DateTime.UtcNow
        };

        var userCreated = await _context.Users.AddAsync(user);
        await _context.SaveChangesAsync();
        return userCreated.Entity;
    }

    public LoginResponse GenerateLoginResponse(int id, string name)
    {
        var authenticationManager = new AuthenticationManager();
        var (token, expiresIn) = authenticationManager.GenerateJwtToken(id, name, _config);

        return new LoginResponse
        {
            Name = name,
            Token = token,
            ExpiresIn = expiresIn,
        };
    }
}