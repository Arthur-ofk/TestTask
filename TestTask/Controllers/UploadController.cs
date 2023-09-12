using Microsoft.AspNetCore.Mvc;
using TestTask.Services;
using TestTask.Services.Contracts;

namespace TestTask.Controllers
{
    [Route("api/blob")]
    [ApiController]
     public class BlobController : ControllerBase
    {
        private readonly IBlobStorageService _blobStorageService;

        public BlobController(BlobStorageService blobStorageService)
        {
            _blobStorageService = blobStorageService;
        }
        
        [HttpPost("/upload")]
        public async Task<IActionResult> UploadFile(IFormFile file)
        {
            if (file == null || file.Length == 0)
            {
                return BadRequest("Invalid file");
            }

            await _blobStorageService.UploadFileToBlobAsync(file);

            return Ok("File uploaded successfully");
        }
        [HttpGet]
        public string  File()
        {
            Console.WriteLine(12321312);
            return "1212";
        }

    }
}
