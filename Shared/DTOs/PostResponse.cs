namespace Snaptwit.Shared.DTOs;

public class PostResponse
{
    public int TotalPages { get; set; }
    public List<PostItem> Posts{ get; set; } = null!;
}

public class PostItem 
{
    public int Id { get; set; }
    public DateTime Date { get; set; }
    public string Content { get; set; } = string.Empty;
    public int? FileType { get; set; }
    public string? FilePath { get; set; }
    public int UserId { get; set; }
    public string UserName { get; set; } = string.Empty;
    public string? ProfilePicturePath { get; set; }
    public int TotalLikes { get; set; }
    public int TotalComments { get; set; }
    public bool IsLiked { get; set; }
}