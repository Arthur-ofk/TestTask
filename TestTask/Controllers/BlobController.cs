using Microsoft.AspNetCore.Mvc;
using TestTask.Services;
using TestTask.Shared;

namespace TestTask.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class BlobController : ControllerBase
    {
        private readonly UploadService _blobStorageService;

        public BlobController(UploadService blobService)
        {
            _blobStorageService = blobService;
        }

        [HttpPost]
        public async Task<IActionResult> UploadFile([FromForm] FileEmailDto fileEmailDto)
        {
            string res = await _blobStorageService.UploadFileAsync(fileEmailDto);

            return Ok(res);
        }
    }
}