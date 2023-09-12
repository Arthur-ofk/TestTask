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
        [Blob("testtask/{rand-guid}", FileAccess.Write)] Stream blobStream,
        
        ILogger log)
        {
            try
            {
                var connectionString = Environment.GetEnvironmentVariable("AzureWebJobsStorage");
                var containerName = "testtask"; 

                var storageAccount = CloudStorageAccount.Parse(connectionString);
                var blobClient = storageAccount.CreateCloudBlobClient();
                var container = blobClient.GetContainerReference(containerName);

                var file = req.Form.Files[0]; // Assuming you're uploading a single file

                if (file != null && file.Length > 0)
                {
                    var blobName = file.FileName; // Use the original file name as the blob name
                    var blockBlob = container.GetBlockBlobReference(blobName);

                    using (var stream = file.OpenReadStream())
                    {
                        await blockBlob.UploadFromStreamAsync(stream);
                    }

                    return new OkObjectResult("File uploaded successfully");
                }
                else
                {
                    return new BadRequestObjectResult("Invalid file");
                }
            }
            catch (Exception ex)
            {
                log.LogError(ex, "Error uploading file");
                return new StatusCodeResult(StatusCodes.Status500InternalServerError);
            }
            //    var file = req.Form.Files["file"];
            //    var recipientEmail = req.Form["email"];

            //    if (file == null || string.IsNullOrEmpty(recipientEmail))
            //    {
            //        return new BadRequestObjectResult("Bad Request: Missing file or recipient email");
            //    }

            //    // Upload the file to Azure Blob Storage
            //    await file.CopyToAsync(blobStream);
            //    blobStream.Close();

            //    // Return a response or redirect to a success page
            //    return new OkObjectResult("File uploaded successfully.");
            //}
            //catch (Exception ex)
            //{
            //    log.LogError($"Error: {ex.Message}");
            //    return new StatusCodeResult(500);
            //}
        }
    }
}
