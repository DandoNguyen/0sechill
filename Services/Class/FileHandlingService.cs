using _0sechill.Data;
using _0sechill.Dto.FileHandlingDto;
using _0sechill.Models.IssueManagement;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;

namespace _0sechill.Services.Class
{
    public class FileHandlingService : IFileHandlingService
    {
        private readonly IConfiguration config;
        private readonly ApiDbContext context;
        private readonly ILogger<FileHandlingService> logger;

        public FileHandlingService(
            IConfiguration config,
            ApiDbContext context,
            ILogger<FileHandlingService> logger)
        {
            this.config = config;
            this.context = context;
            this.logger = logger;
        }

        public async Task<UploadFileResultDto> UploadFile(IFormFile formFile, string ownerId, string rootPath)
        {
            var rootFilePath = "~" + rootPath.Trim();
            if (rootFilePath is null)
                return new UploadFileResultDto()
                {
                    isSucceeded = false,
                    message = "Root Path Not Found"
                };

            if (formFile is null)
            {
                return new UploadFileResultDto()
                {
                    isSucceeded = false,
                    message = "FormFile is null"
                };
            }

            if (!IsValidFileType(formFile))
            {
                return new UploadFileResultDto()
                {
                    isSucceeded = false,
                    message = "formFile type is not supported"
                };
            }

            if (formFile.Length > 0)
            {
                var newRootPath = Path.Combine(rootFilePath, ownerId);


                if (!Directory.Exists(newRootPath))
                {
                    Directory.CreateDirectory(newRootPath);
                }

                //Config Final file Path under newRootPath
                var finalPath = Path.Combine(newRootPath, MakeValidFileName(formFile.Name));

                //try copy to Directory
                try
                {
                    using (var fileStream = new FileStream(finalPath, FileMode.OpenOrCreate))
                    {
                        await formFile.CopyToAsync(fileStream);
                    }
                }
                catch (Exception ex)
                {
                    logger.LogError(ex.Message);
                    return new UploadFileResultDto()
                    {
                        isSucceeded = false,
                        message = $"Error in saving file {formFile.Name}: {ex.Message}"
                    };
                }

                //Create new File model Object
                var newFile = new FilePath();
                switch (rootFilePath)
                {
                    case "App_Data\\FilePaths":
                        newFile.issueId = Guid.Parse(ownerId);
                        break;
                    case "App_Data\\Avatar":
                        newFile.userId = ownerId;
                        break;
                }
                newFile.filePath = finalPath;

                try
                {
                    await context.filePaths.AddAsync(newFile);
                }
                catch (Exception ex)
                {
                    logger.LogError(ex.Message);
                    return new UploadFileResultDto()
                    {
                        isSucceeded = false,
                        message = $"Error in saving File {formFile.Name}'s Info to Database: {ex.Message}"
                    };
                }
                await context.SaveChangesAsync();

                return new UploadFileResultDto()
                {
                    isSucceeded = true,
                    message = newFile.filePath
                };
            }
            return new UploadFileResultDto()
            {
                isSucceeded = false,
                message = $"Error in trying to save File {formFile.Name}"
            };
        }

        //Check for format
        private static bool IsValidFileType(IFormFile file)
        {
            string fileExtension = Path.GetExtension(file.FileName).ToLower();
            switch (fileExtension)
            {
                case ".doc": case ".docx": return true;
                case ".xls": case ".xlsx": return true;
                case ".jpg": case ".png": case ".jpeg": return true;
                default: return false;
            }
        }

        //check for image only
        private static bool IsValidAvatar(IFormFile file)
        {
            string fileExtenstion = Path.GetExtension(file.FileName).ToLower();
            switch (fileExtenstion)
            {
                case ".jpg": case ".png": case ".jpeg": return true;
                default: return false;
            }
        }

        private static string MakeValidFileName(string name)
        {
            string invalidChars = System.Text.RegularExpressions.Regex.Escape(new string(Path.GetInvalidFileNameChars()));
            string invalidRegStr = string.Format(@"([{0}]*\.+$)|([{0}]+) ", invalidChars);
            var newString = System.Text.RegularExpressions.Regex.Replace(name, invalidRegStr, "_").ToString();
            return newString;
        }
    }
}
