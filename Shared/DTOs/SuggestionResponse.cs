namespace Snaptwit.Shared.DTOs;

public class SuggestionResponse
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string? ProfilePicturePath { get; set; }
}