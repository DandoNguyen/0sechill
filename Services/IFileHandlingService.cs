using _0sechill.Dto.FileHandlingDto;

namespace _0sechill.Services
{
    public interface IFileHandlingService
    {
        Task<UploadFileResultDto> UploadFile(IFormFile formFile, string issueId, string author);
    }
}
