using _0sechill.Dto.FileHandlingDto;

namespace _0sechill.Services.Class
{
    public class FileHandlingService //: IFileHandlingService
    {
        private readonly IConfiguration config;

        public FileHandlingService(IConfiguration config)
        {
            this.config = config;
        }
        //public async Task<UploadFileResultDto> UploadFile(IFormFile formFile, string UserName)
        //{
        //    var rootFile = config["FilePaths:IssueFiles"];
        //    if (rootFile is null)
        //        return new UploadFileResultDto()
        //        {
        //            result = false,
        //            message = "Root Path Not Found"
        //        };

        //    if (formFile is null)
        //    {
        //        return new UploadFileResultDto()
        //        {
        //            result = false,
        //            message = "FormFile is null"
        //        };
        //    }
            

        //}

        //Check for format
    }
}
