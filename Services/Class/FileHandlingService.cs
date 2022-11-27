using _0sechill.Data;
using _0sechill.Dto.FileHandlingDto;
using _0sechill.Models;
using _0sechill.Models.IssueManagement;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;

namespace _0sechill.Services.Class
{
    public class FileHandlingService : IFileHandlingService
    {
        private readonly IConfiguration config;
        private readonly ApiDbContext context;
        private readonly UserManager<ApplicationUser> userManager;
        private readonly ILogger<FileHandlingService> logger;

        public FileHandlingService(
            IConfiguration config,
            ApiDbContext context,
            UserManager<ApplicationUser> userManager,
            ILogger<FileHandlingService> logger)
        {
            this.config = config;
            this.context = context;
            this.userManager = userManager;
            this.logger = logger;
        }

        /// <summary>
        /// Public service function that upload files to specific directory
        /// </summary>
        /// <param name="formFile">Target File</param>
        /// <param name="ownerId">ID from either Issue, Avatar or feedback ID from Staff</param>
        /// <param name="rootPath"></param>
        /// <returns>return <paramref name="UploadFileResultDto"/></returns>
        public async Task<UploadFileResultDto> UploadFile(IFormFile formFile, string ownerId, [Required] string rootPath)
        {
            var rootFilePath = "~" + rootPath.Trim();

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
                var finalPath = Path.Combine(newRootPath, MakeValidFileName(formFile.FileName));

                //try copy to Directory

                //await using FileStream fs = new FileStream(newRootPath, FileMode.OpenOrCreate);
                //await formFile.CopyToAsync(fs);

                using (var fileStream = new FileStream(finalPath, FileMode.OpenOrCreate))
                {
                    await formFile.CopyToAsync(fileStream);
                }

                //Create new File model Object
                var newFile = new FilePath();

                newFile.ownerID = ownerId;
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

        //public service function that removes the files in the directory
        public async Task RemoveFiles(string fileId)
        {
            var fileObject = await context.filePaths
                .Where(x => x.ID.Equals(Guid.Parse(fileId)))
                .FirstOrDefaultAsync();
            if (fileObject is not null && fileObject.filePath is not null)
            {
                string[] files = Directory.GetFiles(fileObject.filePath);
                foreach (var file in files)
                {
                    System.IO.File.Delete(file);
                }

                context.filePaths.Remove(fileObject);
                await context.SaveChangesAsync();
            }
        }

        //check for image only // => move to IsValidFileType function to check for image type file too
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

        public async Task<List<string>> getListPaths(string ownerID)
        {
            return await context.filePaths
                .Where(x => x.ownerID.Equals(ownerID))
                .Select(x => x.ID.ToString())
                .ToListAsync();
        }
    }
}
