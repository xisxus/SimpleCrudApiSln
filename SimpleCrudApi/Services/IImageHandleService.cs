namespace SimpleCrudApi.Services
{
    public interface IImageHandleService
    {
        Task<string> SaveFileToFolderAsync(IFormFile file, string? folderPath);
        Task<byte[]> ConvertFileToByteArrayAsync(IFormFile file);
    }
}
