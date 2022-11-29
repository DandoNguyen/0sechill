using _0sechill.Data;
using _0sechill.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.StaticFiles;
using Microsoft.EntityFrameworkCore;

namespace _0sechill.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public class FileHandlingController : ControllerBase
    {
        private readonly ApiDbContext context;
        private readonly IFileHandlingService fileService;

        public FileHandlingController(
            ApiDbContext context,
            IFileHandlingService fileService)
        {
            this.context = context;
            this.fileService = fileService;
        }

        /// <summary>
        /// private function save file to directory
        /// </summary>
        /// <param name="filePath"></param>
        /// <returns></returns>
        private async Task<IActionResult> CopyFileFromDirectoryAsync(string filePath)
        {
            if (!System.IO.File.Exists(filePath))
            {
                return BadRequest("File Not Found");
            }
            var memory = new MemoryStream();
            await using (var stream = new FileStream(filePath, FileMode.Open, FileAccess.Read))
            {
                await stream.CopyToAsync(memory);
            }
            memory.Position = 0;

            var contentType = Path.GetExtension(filePath);
            switch (contentType)
            {
                case ".jpg": case ".png": case ".jpeg":
                    return File(memory, "image/jpeg");
                default:
                    var newContentType = GetContentType(filePath);
                    return File(memory, newContentType);

            }
        }

        /// <summary>
        /// getting existing file
        /// </summary>
        /// <param name="fileId"></param>
        /// <returns></returns>
        [HttpGet, Route("GetFile")]
        [AllowAnonymous]
        public async Task<IActionResult> GetFileAsync(string fileId)
        {
            var filePathString = await context.filePaths
                .Where(x => x.ID.Equals(Guid.Parse(fileId)))
                .Select(x => x.filePath)
                .FirstOrDefaultAsync();
            if (filePathString is null)
            {
                return NotFound();
            }
            return await CopyFileFromDirectoryAsync(filePathString);
        }

        /// <summary>
        /// deleting file from directory
        /// </summary>
        /// <param name="fileId"></param>
        /// <returns></returns>
        [HttpDelete, Route("DeleteFiles")]
        public async Task<IActionResult> RemoveFilesAsync(string fileId)
        {
            var fileObject = await context.filePaths
                .Where(x => x.ID.Equals(Guid.Parse(fileId)))
                .FirstOrDefaultAsync();
            if (fileObject is null)
            {
                return BadRequest("File source not found");
            }

            try
            {
                await fileService.RemoveFiles(fileObject.ToString());
                return Ok("File Removed");
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }

        /// <summary>
        /// private function getting the type of file => see the references
        /// </summary>
        /// <param name="filePath"></param>
        /// <returns></returns>
        private string GetContentType(string filePath)
        {
            var provider = new FileExtensionContentTypeProvider();
            string contentType;

            if (!provider.TryGetContentType(filePath, out contentType))
            {
                contentType = "application/octet-stream";
            }

            return contentType;
        }
    }
}
