namespace TestTask.Services.Contracts
{
    public interface IBlobStorageService
    {
        public  Task UploadFileToBlobAsync(IFormFile file);
    }
}
