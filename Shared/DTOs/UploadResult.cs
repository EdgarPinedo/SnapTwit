namespace Snaptwit.Shared.DTOs;

public class UploadResult
{
    public string Content { get; set; } = string.Empty;
    public int FileType { get; set; }
    public string FilePath { get; set; } = string.Empty;
}