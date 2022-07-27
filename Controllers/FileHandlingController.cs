using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.StaticFiles;

namespace _0sechill.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class FileHandlingController : ControllerBase
    {
        public FileHandlingController()
        {

        }

        [HttpGet, Route("GetFile")]
        public async Task<IActionResult> GetFileAsync(string filePath)
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
                case ".jpg":
                case ".png":
                case ".jpeg":
                    return File(memory, "image/jpeg");
                default:
                    var newContentType = GetContentType(filePath);
                    return File(memory, newContentType);

            }
        }

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
