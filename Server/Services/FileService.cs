using Snaptwit.Shared;
using Snaptwit.Shared.DTOs;

namespace Server.Services;

public class FileService
{
    private readonly IWebHostEnvironment _env;

    public FileService(IWebHostEnvironment env)
    {
        _env = env;
    }

    public async Task<UploadResult> SaveFiles(List<IFormFile> files)
    {
        var file = files[0];
        string fileType = file.ContentType.Split("/")[1];
        string fileName = $"{Path.GetRandomFileName()}.{fileType}";

        Directory.CreateDirectory(Path.Combine(_env.ContentRootPath, "Files"));
        var path = Path.Combine(_env.ContentRootPath, "Files", fileName);

        await using FileStream fs = new (path, FileMode.Create);
        await file.CopyToAsync(fs);
        
        return new UploadResult()
        {
            Content = files[0].FileName == "file" ? string.Empty : files[0].FileName,
            FilePath = $"Files/{fileName}",
            FileType = (fileType == "mp4") ? (int)FileType.video : (int)FileType.image
        };
    }

    public void DeleteFile(string path)
    {
        var rootPath = Path.Combine(_env.ContentRootPath, path);
        File.Delete(rootPath);
    }
}