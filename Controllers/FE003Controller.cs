using _0sechill.Data;
using _0sechill.Dto.FE003.Request;
using _0sechill.Dto.FE003.Response;
using _0sechill.Models;
using _0sechill.Models.IssueManagement;
using _0sechill.Models.LookUpData;
using _0sechill.Services;
using AutoMapper;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace _0sechill.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public class FE003Controller : ControllerBase
    {
        private readonly ApiDbContext context;
        private readonly IMapper mapper;
        private readonly UserManager<ApplicationUser> userManager;
        private readonly IFileHandlingService fileService;
        private readonly IConfiguration config;

        public FE003Controller(
            ApiDbContext context,
            IMapper mapper,
            UserManager<ApplicationUser> userManager,
            IFileHandlingService fileService,
            IConfiguration config)
        {
            this.context = context;
            this.mapper = mapper;
            this.userManager = userManager;
            this.fileService = fileService;
            this.config = config;
        }

        /// <summary>
        /// Get All Issues
        /// </summary>
        /// <param name="dateTime">Start Date search</param>
        /// <returns>Http Response</returns>
        [HttpGet, Route("GetAllIssues")]
        [AllowAnonymous]
        public async Task<IActionResult> GetAllIssues(DateTime dateTime)
        {
            if (dateTime == DateTime.MinValue)
            {
                dateTime = DateTime.Now;
            }
            var listIssues = await context.issues
                .Include(x => x.author)
                .Include(x => x.listCateLookUp)
                .Include(x => x.files)
                .Where(x => x.lastModifiedDate.Date <= dateTime.Date)
                .Take(10).ToListAsync();

            var listIssueDto = new List<IssueDto>();
            listIssueDto = mapper.Map<List<IssueDto>>(listIssues);
            foreach (var issueDto in listIssueDto)
            {
                var issue = await context.issues
                    .Include(x => x.listCateLookUp)
                    .Include(x => x.files)
                    .Where(x => x.ID.Equals(Guid.Parse(issueDto.ID)))
                    .FirstOrDefaultAsync();
                
                foreach (var cateLookUp in issue.listCateLookUp)
                {
                    issueDto.listCategory.Add(cateLookUp.valueString);
                }

                foreach (var filePath in issue.files)
                {
                    issueDto.files.Add(filePath.filePath);
                }
            }

            return Ok(new GetAllIssueDto
            {
                listIssue = listIssueDto,
                searchDate = dateTime
            });
        }

        /// <summary>
        /// public endpoints for creating new Issue
        /// </summary>
        /// <param name="dto"></param>
        /// <returns>Http response</returns>
        [HttpPost, Route("AddNewIssue")]
        public async Task<IActionResult> addNewIssues(CreateIssueDto dto)
        {
            var user = new ApplicationUser();
            var listCateFromLookUp = new List<LookUpTable>();
            var listFileUploadResult = new List<string>();

            //handling current user
            try
            {
                user = await userManager.GetUserAsync(User);
            }
            catch (Exception)
            {
                return BadRequest("Current User Not Found");
            }
            
            //handling content
            var newIssue = new Issues();
            newIssue.content = dto.content;
            newIssue.isPrivate = dto.isPrivate;
            newIssue.author = user;

            //handling category lookup
            if (dto.listCateID.Any())
            {
                foreach (var cateID in dto.listCateID)
                {
                    var cateLookUp = await context.lookUp
                        .Where(x => x.lookUpID.Equals(Guid.Parse(cateID)))
                        .FirstOrDefaultAsync();

                    if (cateLookUp is not null)
                    {
                        listCateFromLookUp.Add(cateLookUp);
                    }
                }
            }

            newIssue.listCateLookUp = listCateFromLookUp;

            //handling files
            if (dto.listFiles.Any())
            {
                foreach (var file in dto.listFiles)
                {
                    var resultDto = await fileService.UploadFile(file, newIssue.ID.ToString(), config["FilePaths:IssueFiles"]);

                    if (!resultDto.isSucceeded)
                    {
                        listFileUploadResult.Add(file.FileName);
                    }
                }
            }

            if (listFileUploadResult.Any())
            {
                return Ok(new
                {
                    message = "Issues Created!\nCouldn't Upload Files: \n",
                    listFile = listFileUploadResult
                });
            }

            return Ok("Issue Created");
        }
    }
}
