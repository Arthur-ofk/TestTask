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

namespace Function
{
    public static class ProcessBlob
    {
        
        [FunctionName("ProcessBlob")]
        public static async Task<IActionResult> Run(
            [HttpTrigger(AuthorizationLevel.Function, "post", Route = null)] HttpRequest req,
        [Blob("testtask/{rand-guid}", FileAccess.Write)] Stream blobStream,
        string email,
        ILogger log)
        {
            try
            {
                var file = req.Form.Files["file"];
                var recipientEmail = req.Form["email"];

                if (file == null || string.IsNullOrEmpty(recipientEmail))
                {
                    return new BadRequestObjectResult("Bad Request: Missing file or recipient email");
                }

                // Upload the file to Azure Blob Storage
                await file.CopyToAsync(blobStream);
                blobStream.Close();

                // Return a response or redirect to a success page
                return new OkObjectResult("File uploaded successfully.");
            }
            catch (Exception ex)
            {
                log.LogError($"Error: {ex.Message}");
                return new StatusCodeResult(500);
            }
        }
    }
}
