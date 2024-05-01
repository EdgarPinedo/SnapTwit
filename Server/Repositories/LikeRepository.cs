using Microsoft.EntityFrameworkCore;
using Server.Data;
using Snaptwit.Shared;

namespace Server.Repositories;

public class LikeRepository
{
    private readonly AppDbContext _context;

    public LikeRepository(AppDbContext context) 
        => _context = context;

    public async Task LikePost(int postId, int userId)
    {
        Like like = new()
        {
            UserId = userId,
            PostId = postId,
            Date = DateTime.UtcNow
        };

        await _context.Likes.AddAsync(like);
        await _context.SaveChangesAsync();
    }

    public async Task DislikePost(int postId, int userId)
    {
        await _context.Likes
            .Where(l => l.PostId == postId && l.UserId == userId)
            .ExecuteDeleteAsync();
    }

    public async Task LikeComment(int commentId, int userId)
    {
        Like like = new()
        {
            UserId = userId,
            CommentId = commentId,
            Date = DateTime.UtcNow
        };

        await _context.Likes.AddAsync(like);
        await _context.SaveChangesAsync();
    }

    public async Task DislikeComment(int commentId, int userId)
    {
        await _context.Likes
            .Where(l => l.CommentId == commentId && l.UserId == userId)
            .ExecuteDeleteAsync();
    }
}