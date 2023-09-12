using Azure.Storage.Blobs;
using TestTask.Services.Contracts;

namespace TestTask.Services
{
    public class BlobStorageService : IBlobStorageService
    {
        private readonly BlobServiceClient _blobServiceClient;
        private readonly string _containerName;

        public BlobStorageService(IConfiguration configuration)
        {
            
            _blobServiceClient = new BlobServiceClient(configuration.GetConnectionString("AzureBlobStorage"));

            
            _containerName = "testtask";
        }

        public async Task UploadFileToBlobAsync(IFormFile file)
        {
           
            BlobContainerClient containerClient = _blobServiceClient.GetBlobContainerClient(_containerName);

            
            string blobName = $"{Guid.NewGuid()}_{Path.GetFileName(file.FileName)}";

            
            BlobClient blobClient = containerClient.GetBlobClient(blobName);

            
            using (Stream stream = file.OpenReadStream())
            {
                await blobClient.UploadAsync(stream, true);
            }
        }
    }
}
