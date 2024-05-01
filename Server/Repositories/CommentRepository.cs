using Microsoft.EntityFrameworkCore;
using Server.Data;
using Snaptwit.Shared;
using Snaptwit.Shared.DTOs;

namespace Server.Repositories;

public class CommentRepository
{
    private readonly AppDbContext _context;

    public CommentRepository(AppDbContext context)
    {
        _context = context;
    }

    public async Task<CommentResponse> GetComments(int? postId, int? commentId, int userId, int page)
    {
        IQueryable<Comment> query = _context.Comments;

        if (postId is not null)
            query = query.Where(c => c.PostId == postId && c.CommentId == null);

        if (commentId is not null)
            query = query.Where(c => c.CommentId == commentId);

        var total = query.Count();
        var totalPages = Math.Ceiling(total/(double)5);
            
        var comments = await query.Select(c => new Comments
        {
            Id = c.Id,
            Content = c.Content,
            Date = c.Date,
            UserId = c.UserId,
            UserName = c.User.Name,
            ProfilePicturePath = c.User.ProfilePicturePath,
            PostId = c.PostId,
            TotalLikes = c.Likes.Count(),
            TotalReplies = c.Replies.Count(),
            IsLiked = c.Likes.Any(l => l.UserId == userId)
        })
        .OrderByDescending(c => c.Date)
        .Skip((page-1)*5)
        .Take(5)
        .ToListAsync();

        return new CommentResponse
        {
            TotalPages = (int)totalPages,
            Replies = comments
        };
    }

    public async Task Reply(CommentRequest reply, int userId)
    {
        Comment comment = new()
        {
            Content = reply.Content,
            UserId = userId,
            CommentId = reply.CommentId,
            PostId = reply.PostId,
            Date = reply.Date
        };

        await _context.Comments.AddAsync(comment);
        await _context.SaveChangesAsync();
    }
}