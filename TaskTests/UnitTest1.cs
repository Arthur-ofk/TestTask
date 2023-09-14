using AutoMapper;
using Azure.Storage.Blobs.Models;
using Azure.Storage.Blobs.Specialized;
using Azure.Storage.Blobs;
using Microsoft.Extensions.Configuration;
using TestTask.Models;
using TestTask.Services;
using TestTask.Shared;
using Moq;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using TestTask.ServicesContracts;
using System.Collections;


namespace TaskTests
{
    [TestClass]
    public class UnitTest1
    {
        private IMapper _mapperMock;
       
        private IUploadService _uploadService;

        [TestInitialize]
        public void Initialize()
        {
            var configurationMock = new Mock<IConfiguration>();
            _mapperMock = ResolveObjectForTesting<IMapper>();

            var configuration = new ConfigurationBuilder()
                      .AddInMemoryCollection(new Dictionary<string, string>
                    {
                        { "ConnectionStrings:AzureBlobStorage", "DefaultEndpointsProtocol=https;AccountName=arturblobcontainers;AccountKey=JKkJ0/TTolH2xkIOjl617+fJujaLeWRszo4WVf5iAj6aLam22yZC6uug/WK+SWu8OGFCrFulVHvF+AStKaAbtg==;EndpointSuffix=core.windows.net" }
                    })
                     .Build();
            _uploadService = ResolveObjectForTesting<IUploadService>();

            
           
        }

        [TestMethod]
        public async Task UploadFileAsync_ValidFile_ReturnsSuccessMessage()
        {

            //var file = new Mock<IFormFile>().Object;
            
              string filePath = $"{Directory.GetCurrentDirectory()}\\Resources\\Test.txt";

            
            var fileStream = new FileStream(filePath, FileMode.Open);


            var fileMock = new Mock<IFormFile>();
            fileMock.Setup(f => f.FileName).Returns("Test.docx");
            fileMock.Setup(f => f.Length).Returns(fileStream.Length);
            fileMock.Setup(f => f.OpenReadStream()).Returns(fileStream);

            //string filePath = "C:\\path\\to\\your\\local\\file.txt";

            //// Створення імітації IFormFile
            //var formFile = file.CreateFormFile(filePath);

            var email = "test@example.com";
            var fileEmailDto = new FileEmailDto(fileMock.Object, email);
            var reult = await _uploadService.UploadFileAsync(fileEmailDto);


            //var mockedBlobClient = new Mock<BlockBlobClient>();
            //mockedBlobClient.Setup(blob => blob.UploadAsync(It.IsAny<Stream>(), It.IsAny<BlobUploadOptions>(), null))
            //    .ReturnsAsync(new BlobContentInfo());

            //var mockedBlobContainerClient = new Mock<BlobContainerClient>();
            //mockedBlobContainerClient.Setup(client => client.GetBlobClient(It.IsAny<string>()))
            //    .Returns(mockedBlobClient.Object);

            //_mapperMock.Setup(mapper => mapper.Map<FileEmail>(fileEmailDto))
            //    .Returns(new FileEmail { /* Set properties as needed */ });

            //var blobServiceClient = new Mock<BlobServiceClient>();
            //blobServiceClient.Setup(client => client.GetBlobContainerClient(It.IsAny<string>()))
            //    .Returns(mockedBlobContainerClient.Object);

            //var uploadService = new UploadService(_configuration, _mapperMock.Object);

            //// Act
            //var result = await uploadService.UploadFileAsync(fileEmailDto);

            //// Assert
            //Assert.IsTrue(result.Contains("uploaded successfully"));
        }
        private T ResolveObjectForTesting<T> ()
        {

            var serviceCollection = new ServiceCollection();
            Microsoft.Extensions.Configuration.IConfiguration configuration = new ConfigurationBuilder()
            .SetBasePath(Directory.GetCurrentDirectory())
            .AddJsonFile("appsettings.test.json", optional: false, reloadOnChange: true)
            .Build();
            serviceCollection.AddSingleton<Microsoft.Extensions.Configuration.IConfiguration>(configuration);

            serviceCollection.AddServices();

            var serviceProvider = serviceCollection.BuildServiceProvider();

            var service = serviceProvider.GetService<T>();
            return service;
        }

    }
}