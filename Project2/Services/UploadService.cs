using AutoMapper;
using Azure.Storage.Blobs;
using Microsoft.WindowsAzure.Storage;
using TestTask.Models;
using TestTask.ServicesContracts;
using TestTask.Shared;

namespace TestTask.Services
{
    public class UploadService 
    {
        private readonly BlobServiceClient _blobServiceClient;
        private readonly BlobContainerClient _containerClient;
        private readonly IMapper _mapper;
        private readonly string _connectionString;


        public UploadService(IConfiguration configuration, IMapper mapper)
        {
            _connectionString = configuration.GetConnectionString("AzureBlobStorage");
            _blobServiceClient = new BlobServiceClient(_connectionString);
            _containerClient = _blobServiceClient.GetBlobContainerClient("testtask");
            _mapper = mapper;
        }


        public async Task<string> UploadFileAsync(FileEmailDto fileEmailDto)
        {
            var fileEmail = _mapper.Map<FileEmail>(fileEmailDto);
            try
            {
                var storageAccount = CloudStorageAccount.Parse(_connectionString);
                var blobClient = storageAccount.CreateCloudBlobClient();
                var containerName = "testtask";
                var container = blobClient.GetContainerReference(containerName);
                string fileName = $"{Guid.NewGuid()}-{fileEmail.File.FileName}";

                // BlobClient blobClient = _containerClient.GetBlobClient(fileName);
                await container.CreateIfNotExistsAsync();

                var filename = $"{Guid.NewGuid().ToString()}.docx";
                var blob = container.GetBlockBlobReference(filename);

                blob.Metadata["Email"] = fileEmail.Email;

                using (var fileStream = fileEmail.File.OpenReadStream())
                {
                    await blob.UploadFromStreamAsync(fileStream);
                }
                return $"File '{filename}' uploaded successfully for email '{fileEmail.Email}'.";
            }
            catch (Exception ex)
            {
                
                throw ex;
            }
        }
    }
}
