namespace Snaptwit.Shared.DTOs;

public class ProfileResponse
{
    public string Name { get; set; } = string.Empty;
    public DateTime JoinedDate { get; set; }
    public string? ProfilePicturePath { get; set; }
    public string? CoverPicturePath { get; set; }
    public bool IsFollowed { get; set; }
    public int TotalFollowing { get; set; }
    public int TotalFollowers { get; set; }
}