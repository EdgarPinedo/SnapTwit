using Server.Data;
using Microsoft.EntityFrameworkCore;
using Snaptwit.Shared;
using Snaptwit.Shared.DTOs;
using Server.Services;

namespace Server.Repositories;

public class PostsRepository
{
    private readonly AppDbContext _context;
    private readonly FileService _fileService;

    public PostsRepository(AppDbContext context, FileService fileService)
    {
        _context = context;
        _fileService = fileService;
    }

    public async Task<PostResponse> GetPosts(int id, bool explore, int page)
    {
        var followingIds = await _context.Followers
            .Where(f => f.FollowingUserId == id)
            .Select(f => f.FollowedUserId)
            .ToListAsync();

        IQueryable<Post> query = _context.Posts;

        if (explore == false)
            query = query.Where(p => followingIds.Contains(p.UserId));
        else
            query = query.Where(p => !followingIds.Contains(p.UserId) && p.UserId != id);

        int TotalPosts = query.Count();
        int TotalPages = (int)Math.Ceiling(TotalPosts/(double)10);

        var posts = await query
            .OrderByDescending(p => p.Date)
            .Skip((page - 1) * 10)
            .Take(10)
            .Select(p => new PostItem
            {
                Id = p.Id,
                Date = p.Date,
                Content = p.Content,
                FileType = p.FileType,
                FilePath = p.FilePath,
                UserId = p.UserId,
                UserName = p.User.Name,
                ProfilePicturePath = p.User.ProfilePicturePath,
                TotalLikes = p.Likes.Count(),
                TotalComments = p.Comments.Count(),
                IsLiked = p.Likes.Any(x => x.UserId == id)
            })
            .ToListAsync();
        
        return new PostResponse()
        {
            TotalPages = TotalPages,
            Posts = posts
        };
    }

    public async Task<PostResponse> GetUserPostsAsync(int id, int userId, int page)
    {
        IQueryable<Post> query = _context.Posts
            .Where(p => p.UserId == id);

        int TotalPosts = query.Count();
        int TotalPages = (int)Math.Ceiling(TotalPosts/(double)10);

        var posts = await query
            .OrderByDescending(p => p.Date)
            .Skip((page - 1) * 10)
            .Take(10)
            .Select(p => new PostItem
            {
                Id = p.Id,
                Date = p.Date,
                Content = p.Content,
                FileType = p.FileType,
                FilePath = p.FilePath,
                UserId = p.UserId,
                UserName = p.User.Name,
                ProfilePicturePath = p.User.ProfilePicturePath,
                TotalLikes = p.Likes.Count(),
                TotalComments = p.Comments.Count(),
                IsLiked = p.Likes.Any(x => x.UserId == userId)
            })
            .ToListAsync();

        return new PostResponse()
        {
            TotalPages = TotalPages,
            Posts = posts
        };
    }

    public async Task CreatePost(string content, int userId)
    {
        Post post = new() 
        {
            Content = content,
            Date = DateTime.UtcNow,
            UserId = userId
        };

        await _context.Posts.AddAsync(post);
        await _context.SaveChangesAsync();
    }

    public async Task<UploadResult> CreatePostWithMedia(List<IFormFile> files, int userId)
    {
        var result = await _fileService.SaveFiles(files);

        Post post = new() 
        {
            Content = result.Content,
            FileType = result.FileType,
            FilePath = result.FilePath,
            Date = DateTime.UtcNow,
            UserId = userId
        };

        await _context.Posts.AddAsync(post);
        await _context.SaveChangesAsync();
        return result;
    }

    public async Task<List<MediaFile>> GetFilesInfo(int id, int page)
        => await _context.Posts.Where(p => p.UserId == id && p.FilePath != null)
            .OrderByDescending(p => p.Date)
            .Skip((page-1)*6)
            .Take(6)
            .Select(p => new MediaFile
            {
                Url = p.FilePath,
                FileType = p.FileType
            }).ToListAsync();

    public async Task<MediaResponse> GetMediaFromUser(int id, int page)
    {
        int totalFiles = await _context.Posts
                            .Where(p => p.UserId == id && p.FilePath != null)
                            .CountAsync();
        
        int totalPages = (int)Math.Ceiling(totalFiles/(double)6);

        MediaResponse mediaResponse = new()
        {
            TotalPages = totalPages,
            Files = await GetFilesInfo(id, page)
        };

        return mediaResponse;
    }
}