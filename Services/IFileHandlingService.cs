using _0sechill.Dto.FileHandlingDto;

namespace _0sechill.Services
{
    public interface IFileHandlingService
    {
        UploadFileResultDto UploadFile(IFormFile formFile);
    }
}
