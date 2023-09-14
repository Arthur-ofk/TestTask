using Microsoft.AspNetCore.Mvc;
using TestTask.Services;
using TestTask.ServicesContracts;
using TestTask.Shared;

namespace TestTask.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class BlobController : ControllerBase
    {
        private readonly IUploadService _blobStorageService;

        public BlobController(IUploadService blobService)
        {
            _blobStorageService = blobService;
        }

        [HttpPost]
        public async Task<IActionResult> UploadFile([FromForm] FileEmailDto fileEmailDto)
        {
            string res = await _blobStorageService.UploadFileAsync(fileEmailDto);

            return Ok(res) ; ;
        }
    }
}