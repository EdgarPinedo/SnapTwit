using Microsoft.EntityFrameworkCore;
using Server.Data;
using Server.Services;
using Snaptwit.Shared;
using Snaptwit.Shared.DTOs;

namespace Server.Repositories;

public class UserRepository
{
    private readonly AppDbContext _context;
    private readonly FileService _fileService;

    public UserRepository(AppDbContext context, FileService fileService)
    {
        _context = context;
        _fileService = fileService;
    }

    public async Task<List<SuggestionResponse>> GetRandomAsync(int id)
    {
        return await _context.Users
            .Where(u => u.Id != id && !u.Followers.Select(f => f.FollowingUserId).Contains(id))
            .Select(u => new SuggestionResponse 
            {
                Id = u.Id,
                Name = u.Name,
                ProfilePicturePath = u.ProfilePicturePath
            })
            .OrderBy(u => EF.Functions.Random())
            .Take(5)
            .ToListAsync();
    }

    public async Task<ProfileResponse?> GetProfile(int id, int reqUserId)
        => await _context.Users
            .Where(u => u.Id == id)
            .Select(u => new ProfileResponse
            {
                Name = u.Name,
                JoinedDate = u.JoinedDate,
                ProfilePicturePath = u.ProfilePicturePath,
                CoverPicturePath = u.CoverPicturePath,
                IsFollowed = u.Followers.Any(x => x.FollowingUserId == reqUserId),
                TotalFollowers = u.Followers.Count(),
                TotalFollowing = u.Following.Count()
            })
            .FirstOrDefaultAsync();

    public async Task<UploadResult> UpdateProfilePicture(List<IFormFile> files, int userId)
    {
        var result = await _fileService.SaveFiles(files);
        var user = await _context.Users.FirstOrDefaultAsync(u => u.Id == userId);
        
        if(user?.ProfilePicturePath is not null)
            _fileService.DeleteFile(user.ProfilePicturePath);

        user!.ProfilePicturePath = result.FilePath;
        await _context.SaveChangesAsync();
        return result;
    }

    public async Task<UploadResult> UpdateCoverPicture(List<IFormFile> files, int userId)
    {
        var result = await _fileService.SaveFiles(files);
        var user = await _context.Users.FirstOrDefaultAsync(u => u.Id == userId);
        
        if(user?.CoverPicturePath is not null)
            _fileService.DeleteFile(user.CoverPicturePath);

        user!.CoverPicturePath = result.FilePath;
        await _context.SaveChangesAsync();
        return result;
    }

    public async Task FollowUser(int UserId, int followedId)
    {
        Followers follow = new()
        {
            FollowingUserId = UserId,
            FollowedUserId = followedId
        };
        await _context.Followers.AddAsync(follow);
        await _context.SaveChangesAsync();
    }

    public async Task UnfollowUser(int UserId, int followedId)
    {
        await _context.Followers
            .Where(f => f.FollowingUserId == UserId && f.FollowedUserId == followedId)
            .ExecuteDeleteAsync();
        await _context.SaveChangesAsync();
    }

    public async Task<List<SuggestionResponse>> SearchUser(string query)
    {
        return await _context.Users
            .Where(u => u.Name.Contains(query))
            .Select(u => new SuggestionResponse
            {
                Id = u.Id,
                Name = u.Name,
                ProfilePicturePath = u.ProfilePicturePath
            })
            .ToListAsync();
    }
}