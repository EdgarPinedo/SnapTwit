namespace Snaptwit.Shared.DTOs;

public class CommentRequest
{
    public string Content { get; set; } = string.Empty;
    public int PostId { get; set; }
    public int? CommentId { get; set; }
    public DateTime Date { get; set; }
}