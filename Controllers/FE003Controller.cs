using _0sechill.Data;
using _0sechill.Dto.FE003.Request;
using _0sechill.Dto.FE003.Response;
using _0sechill.Dto.MailDto;
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
using System.ComponentModel.DataAnnotations;

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
        private readonly IMailService mailService;

        public FE003Controller(
            ApiDbContext context,
            IMapper mapper,
            UserManager<ApplicationUser> userManager,
            IFileHandlingService fileService,
            IConfiguration config,
            IMailService mailService)
        {
            this.context = context;
            this.mapper = mapper;
            this.userManager = userManager;
            this.fileService = fileService;
            this.config = config;
            this.mailService = mailService;
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
        public async Task<IActionResult> addNewIssues([FromForm]CreateIssueDto dto)
        {
            var user = new ApplicationUser();
            var listCateFromLookUp = new List<LookUpTable>();
            var listFileUploadResult = new List<string>();

            //handling current user
            try
            {
                user = await userManager.FindByIdAsync(this.User.FindFirst("ID").Value);
            }
            catch (Exception)
            {
                return BadRequest("Current User Not Found");
            }

            if (user is null)
            {
                return Unauthorized();
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

            if (ModelState.IsValid)
            {
                try
                {
                    await context.issues.AddAsync(newIssue);
                    await context.SaveChangesAsync();
                }
                catch (Exception ex)
                {
                    return new JsonResult($"Create Issues failed\nError: {ex.Message}") { StatusCode = 500 };
                }
            }

            //send notification to blockManager
            //var blockManagerEmail = user.block.blockManager.Email;
            //if (blockManagerEmail != null)
            //{
            //    SendEmailAsync(newIssue, blockManagerEmail);
            //} 

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

        private void SendEmailAsync(Issues newIssue, string receiverEmail)
        {
            MailContent newMailContent = new MailContent()
            {
                ToEmail = receiverEmail,
                Subject = "New Issue has been created!",
                Body = $"User {newIssue.author.lastName} {newIssue.author.firstName} has created a new Issue on {newIssue.createdDate}!"
            };
            mailService.SendMailAsync(newMailContent);
        }

        /// <summary>
        /// this method is to get isses Details
        /// </summary>
        /// <param name="ID"></param>
        /// <returns></returns>
        [HttpGet, Route("GetIssueDetail")]
        public async Task<IActionResult> GetIssueDetail([Required] string ID)
        {
            var existIssue = await context.issues
                .Include(x => x.author)
                .Include(x => x.listCateLookUp)
                .Include(x => x.statusLookUp)
                .FirstOrDefaultAsync(x => x.ID.Equals(Guid.Parse(ID)));
            if (existIssue is null)
            {
                return BadRequest("Issue Not Found");
            }

            var issueDto = new IssueDto();
            issueDto = mapper.Map<IssueDto>(existIssue);

            //handling cate
            foreach (var cate in existIssue.listCateLookUp)
            {
                issueDto.listCategory.Add(cate.valueString);
            }

            //handling status
            issueDto.status = existIssue.statusLookUp.valueString;

            return Ok(issueDto);
        }

        /// <summary>
        /// this is to get all issue by status
        /// </summary>
        /// <param name="statusID"></param>
        /// <returns></returns>
        [HttpGet]
        public async Task<IActionResult> GetAllIssuesByStatus([Required] string statusID)
        {
            var listIssues = await context.issues
                .Include(x => x.statusLookUp)
                .Include(x => x.listCateLookUp)
                .Where(x => x.statusLookUp.Equals(Guid.Parse(statusID)))
                .ToListAsync();

            if (!listIssues.Any())
            {
                return new JsonResult(new
                {
                    message = "No Issue Found!"
                })
                { StatusCode = 204 };
            }

            var listIssueDto = new List<IssueDto>();
            foreach (var issue in listIssues)
            {
                var newIssueDto = new IssueDto();

                newIssueDto = mapper.Map<IssueDto>(issue);

                //map Cate Lookup
                foreach (var cate in issue.listCateLookUp)
                {
                    newIssueDto.listCategory.Add(cate.valueString);
                }
                
                //Map Status Look Up
                newIssueDto.status = issue.statusLookUp.valueString;

                listIssueDto.Add(newIssueDto);
            }

            return Ok(listIssueDto);
        }
    }
}
