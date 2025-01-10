
namespace SimpleCrudApi.Services
{
    public class ImageHandlerService : IImageHandleService
    {
        private readonly IWebHostEnvironment _environment;

        public ImageHandlerService(IWebHostEnvironment environment)
        {
            _environment = environment;
        }

        public async Task<string> SaveFileToFolderAsync(IFormFile file, string? folderPath)
        {
            if (file == null || file.Length == 0)
                throw new ArgumentException("File is null or empty.");

            if (!Directory.Exists(_environment.WebRootPath + "Upload"))
            {
                Directory.CreateDirectory(_environment.WebRootPath + "Upload");
            }


            folderPath = _environment.WebRootPath + "Upload";
            string uniqueFileName = Guid.NewGuid() + Path.GetExtension(file.FileName);
            string filePath = Path.Combine(folderPath, uniqueFileName);

            Directory.CreateDirectory(folderPath); // Ensure the folder exists

            using (var fileStream = new FileStream(filePath, FileMode.Create))
            {
                await file.CopyToAsync(fileStream);
            }

            return filePath; // Return the saved file path
        }

        public async Task<byte[]> ConvertFileToByteArrayAsync(IFormFile file)
        {
            if (file == null || file.Length == 0)
                throw new ArgumentException("File is null or empty.");

            using (var memoryStream = new MemoryStream())
            {
                await file.CopyToAsync(memoryStream);
                return memoryStream.ToArray(); // Return the byte array
            }
        }
    }
}
