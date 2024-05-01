namespace Snaptwit.Shared.DTOs;

public class MediaResponse
{
    public int TotalPages { get; set; }
    public List<MediaFile> Files { get; set; } = null!;
}

public class MediaFile
{
    public string? Url { get; set; }
    public int? FileType { get; set; }
}