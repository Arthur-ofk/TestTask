using TestTask.Shared;

namespace TestTask.ServicesContracts
{
    public interface IUploadService
    {
        Task<string> UploadFileAsync(FileEmailDto fileEmailDto);
    }
}
