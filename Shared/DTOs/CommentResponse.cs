namespace Snaptwit.Shared.DTOs;

public class CommentResponse
{
    public int TotalPages { get; set; }
    public List<Comments> Replies { get; set; } = null!;
}

public class Comments
{
    public int Id { get; set; }
    public string Content { get; set; } = string.Empty;
    public int UserId { get; set; }
    public string UserName { get; set; } = string.Empty;
    public string? ProfilePicturePath { get; set; }
    public int PostId { get; set; }
    public DateTime Date { get; set; }
    public int TotalLikes { get; set; }
    public int TotalReplies { get; set; }
    public bool IsLiked { get; set; }
}