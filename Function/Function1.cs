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
using Newtonsoft.Json.Linq;
using System.IO.Compression;
using System.Collections;
using System.Collections.Generic;
using Microsoft.WindowsAzure.Storage;
using System.Reflection.Metadata;
using Azure.Storage;
using Microsoft.WindowsAzure.Storage.Blob;

namespace Function
{
    public   static  class Function1
    {

        [FunctionName("Function1")]
        public static void Run([BlobTrigger("testtask/{name}", Connection = "AzureWebJobsStorage")] Stream blobStream, string name
         , IDictionary<string, string> metadata,
         ILogger log)
        {
            var blobServiceClient = new BlobServiceClient(Environment.GetEnvironmentVariable("AzureWebJobsStorage"));
            var containerClient = blobServiceClient.GetBlobContainerClient("testtask");
            var blobClient = containerClient.GetBlobClient(name);
            string recipientEmail = metadata["email"];
            
            var sasToken = GetBlobSasToken(name);

            
            var securedUrl = GetSecuredBlobUrl(name, sasToken , blobClient);

            
            SendEmail(recipientEmail, name, securedUrl, log);

            log.LogInformation($"Processed blob {name}");
            

        }

        private static string GetSecuredBlobUrl(string name, string sasToken, BlobClient blobClient)
        {
            
            return $"{blobClient.Uri}?{sasToken}";
        }

        private static string GetBlobSasToken(string name)
        {

            string storageAccountName = "arturblobcontainers";
            string storageAccountKey = "JKkJ0/TTolH2xkIOjl617+fJujaLeWRszo4WVf5iAj6aLam22yZC6uug/WK+SWu8OGFCrFulVHvF+AStKaAbtg==";

           
            //var serviceClient = new BlobServiceClient(
            //    new Uri($"https://{storageAccountName}.blob.core.windows.net"),
            var storageKeyCredential=   new StorageSharedKeyCredential(storageAccountName, storageAccountKey);

            
           // var containerClient = serviceClient.GetBlobContainerClient("testtask");
            

            
            var sasBuilder = new BlobSasBuilder
            {
                BlobContainerName = "testtask",
                BlobName = name,
                Resource = "b",
                StartsOn = DateTimeOffset.UtcNow,
                ExpiresOn = DateTimeOffset.UtcNow.AddHours(1)
              
            };
            sasBuilder.SetPermissions(BlobSasPermissions.Read);
            var sasToken = sasBuilder.ToSasQueryParameters(storageKeyCredential).ToString();

            //var sasToken = blobClient.GenerateSasUri(sasBuilder);

            return sasToken;
        }
        private static void SendEmail(string recipientEmail, string blobName, string securedUrl, ILogger log)
        {
            try
            {
                
                string smtpServer = Environment.GetEnvironmentVariable("SmtpServer");
                int smtpPort = Convert.ToInt32(Environment.GetEnvironmentVariable("SmtpPort"));
                string smtpUsername = Environment.GetEnvironmentVariable("SmtpUsername");
                string smtpPassword = Environment.GetEnvironmentVariable("SmtpPassword");
                string senderEmail = Environment.GetEnvironmentVariable("SenderEmail");

                
                using (SmtpClient client = new SmtpClient(smtpServer))
                {
                    client.Port = smtpPort;
                    client.Credentials = new NetworkCredential(smtpUsername, smtpPassword);
                    
                    client.EnableSsl = true;
                    client.DeliveryMethod = SmtpDeliveryMethod.Network;


                    using (MailMessage message = new MailMessage(senderEmail, recipientEmail))
                    {

                        message.Subject = "File Upload Notification";
                        message.Body = $@"<html>
                                              <body>
                                                   <p>Click the link below to download your file:</p>
                                                    <p><a href='{securedUrl}'>Download File</a></p>
                                               </body>
                                         </html>";
                        message.IsBodyHtml = true;
                        client.Send(message);
                       

                        log.LogInformation("Email sent successfully.");
                    }
                }
            }
            catch (Exception ex)
            {
                log.LogError($"Error sending email: {ex.Message}");
            }
        }
    }
}
