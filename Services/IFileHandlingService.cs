using _0sechill.Dto.FileHandlingDto;

namespace _0sechill.Services
{
    public interface IFileHandlingService
    {
        /// <summary>
        /// Public service function that upload files to specific directory
        /// </summary>
        /// <param name="formFile">Target File</param>
        /// <param name="ownerId">ID from either Issue, Avatar or feedback ID from Staff</param>
        /// <param name="rootPath"></param>
        /// <returns>return <paramref name="UploadFileResultDto"/></returns>
        Task<UploadFileResultDto> UploadFile(IFormFile formFile, string ownerId, string rootPath);
        Task RemoveFiles(string fileId);
        Task<List<string>> getListPaths(string ownerID);
    }
}
