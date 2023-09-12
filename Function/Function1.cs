using System;
using System.IO;
using Azure.Storage.Blobs;
using Azure.Storage.Sas;
using System.Net.Mail;
using System.Net;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Host;
using Microsoft.Extensions.Logging;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.Azure.WebJobs.Extensions.Http;

namespace Function
{
    public   static  class Function1
    {

        [FunctionName("Function1")]
        public static void Run([BlobTrigger("testtask/{name}", Connection = "AzureWebJobsStorage")] Stream myBlob, string name,
             ILogger log)
        {
            //log.LogInformation($"C# Blob trigger function processed blob\n Name:{name} \n  ContentType: {myBlob.Length} Bytes");
           
            // Generate a SAS token for the blob with a 1-hour validity
            var blobServiceClient = new BlobServiceClient(Environment.GetEnvironmentVariable("AzureWebJobsStorage"));
            var blobClient = blobServiceClient.GetBlobContainerClient("testtask").GetBlobClient(name);
            var sasBuilder = new BlobSasBuilder
            {
                BlobContainerName = "testtask",
                BlobName = name,
                ExpiresOn = DateTimeOffset.UtcNow.AddHours(1),
                Protocol = SasProtocol.Https
            };
            sasBuilder.SetPermissions(BlobSasPermissions.Read);
            var sasToken = blobClient.GenerateSasUri(sasBuilder);

            // Send an email with the SAS token URL using SMTP
            var smtpClient = new SmtpClient("smtp.gmail.com.")
            {
                Port = 587,
                Credentials = new NetworkCredential("arthauz18@gmail.com", "testtask"),
                EnableSsl = true,
            };

           

           

            
        }
    }
}
