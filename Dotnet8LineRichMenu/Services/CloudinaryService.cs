using CloudinaryDotNet;
using CloudinaryDotNet.Actions;

namespace Dotnet8LineRichMenu.Services;

public class CloudinaryService
{
    private readonly Cloudinary _cloudinary;
    private readonly HttpClient _httpClient;

    public CloudinaryService(Cloudinary cloudinary, IHttpClientFactory httpClientFactory)
    {
        _cloudinary = cloudinary;
        _httpClient = httpClientFactory.CreateClient();
    }
    
    public async Task<string> UploadSingleFileAsync(string url, string publicId = null, int maxRetries = 3, int retryDelayMs = 1000)
    {
        for (int attempt = 1; attempt <= maxRetries; attempt++)
        {
            try
            {
                // TODO: Download From Url and upload to Cloudinary
                var imageBytes = await _httpClient.GetByteArrayAsync(url);
                using var stream = new MemoryStream(imageBytes);
                var uploadParams = new ImageUploadParams()
                {
                    File = new FileDescription(Guid.NewGuid().ToString(), stream)
                };
                
                var uploadResult = await _cloudinary.UploadAsync(uploadParams);
            
                if (uploadResult.StatusCode == System.Net.HttpStatusCode.OK)
                {
                    return uploadResult.SecureUrl.AbsoluteUri;
                }
            
                Console.WriteLine($"Upload attempt {attempt} failed. Status code: {uploadResult.StatusCode}");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"An error occurred during file upload (attempt {attempt}): {ex.Message}");
            }

            if (attempt < maxRetries)
            {
                Console.WriteLine($"Retrying in {retryDelayMs/1000} seconds...");
                await Task.Delay(retryDelayMs);
            }
            else
            {
                Console.WriteLine($"Max retries ({maxRetries}) reached. Upload failed.");
            }
        }

        throw new Exception($"Failed to upload file after {maxRetries} attempts.");
    }
    
}