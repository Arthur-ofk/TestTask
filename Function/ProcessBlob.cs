using System;
using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System.IO.Compression;
using Microsoft.WindowsAzure.Storage;

namespace Function
{
    public static class ProcessBlob
    {

        [FunctionName("ProcessBlob")]
        public static async Task<IActionResult> Run(
            [HttpTrigger(AuthorizationLevel.Anonymous, "post", Route = "ProcessBlob")] HttpRequest req,
        //[Blob("testtask/{rand-guid}", FileAccess.Write)] Stream blobStream,

        ILogger log)
        {

            try
            {

                var formCollection = await req.ReadFormAsync();


                var file = formCollection.Files.GetFile("file");
                var fileContent = file.OpenReadStream();


                string email = formCollection["email"];


                var filename = Guid.NewGuid().ToString();


                var storageConnectionString = Environment.GetEnvironmentVariable("AzureWebJobsStorage");


                var storageAccount = CloudStorageAccount.Parse(storageConnectionString);


                var blobClient = storageAccount.CreateCloudBlobClient();


                var containerName = "testtask";
                var container = blobClient.GetContainerReference(containerName);


                await container.CreateIfNotExistsAsync();


                var blob = container.GetBlockBlobReference(filename);

                blob.Metadata["Email"] = email;
                await blob.UploadFromStreamAsync(fileContent);



                return new OkObjectResult($"File '{filename}' uploaded successfully for email '{email}'.");
            }
            catch (Exception ex)
            {
                log.LogError(ex, "An error occurred while uploading the file.");
                return new BadRequestObjectResult("An error occurred while uploading the file.");
            }





        }
    }
}
