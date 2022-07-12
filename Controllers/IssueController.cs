﻿using _0sechill.Data;
using _0sechill.Dto.Issues.Requests;
using _0sechill.Dto.Issues.Response;
using _0sechill.Dto.MailDto;
using _0sechill.Models.IssueManagement;
using _0sechill.Services;
using _0sechill.Static;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace _0sechill.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    //[Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public class IssueController : ControllerBase
    {
        private readonly ApiDbContext context;
        private readonly ITokenService tokenService;
        private readonly IFileHandlingService fileService;
        private readonly IConfiguration config;
        private readonly IMailService mailService;
        private readonly IMapper mapper;
        private readonly ILogger<IssueController> logger;

        public IssueController(
            ApiDbContext context,
            ITokenService tokenService,
            IFileHandlingService fileService,
            IConfiguration config,
            IMailService mailService,
            IMapper mapper,
            ILogger<IssueController> logger)
        {
            this.context = context;
            this.tokenService = tokenService;
            this.fileService = fileService;
            this.config = config;
            this.mailService = mailService;
            this.mapper = mapper;
            this.logger = logger;
        }

        //TEST 
        //[HttpGet, Route("GetProfileUser")]
        //public async Task<IActionResult> GetProfileUserAsync([FromHeader] string Authorization)
        //{
        //    var username = await DecodeToken(Authorization);
        //}

        [HttpGet, Route("GetAllIssue")]
        public async Task<IActionResult> GetAllIssueAsync()
        {
            var listExistIssue = await context.issues
                .Include(x => x.author)
                .Include(x => x.category)
                .Where(x => x.status.Trim().ToLower().Equals(IssueStatus.verified.Trim().ToLower()))
                .ToListAsync();
            if (listExistIssue.Count.Equals(0))
                return BadRequest("No Issues available!");

            var listIssueDto = new List<IssueDto>();
            foreach (var issue in listExistIssue)
            {
                var issueDto = mapper.Map<IssueDto>(issue);
                issueDto.cateName = issue.category.cateName;
                issueDto.authorName = $"{issue.author.firstName} {issue.author.lastName}";
                listIssueDto.Add(issueDto);
            }
            return Ok(listIssueDto);
        }

        [HttpPost, Route("GiveFeedback")]
        public async Task<IActionResult> GiveFeedbackAsync(IssueReviewResultDto dto)
        {
            var existIssue = await context.issues
                .FirstOrDefaultAsync(x => x.ID.Equals(Guid.Parse(dto.issueId)));
            if (existIssue == null) return BadRequest("No Issue Found");

            existIssue.feedback = dto.feedback;

            switch (dto.isVerified)
            {
                case true:
                    existIssue.status = IssueStatus.verified.Trim().ToLower();
                    break;
                case false:
                    existIssue.status = IssueStatus.rejected.Trim().ToLower();
                    break;
            }

            context.issues.Update(existIssue);
            await context.SaveChangesAsync();
            return Ok("FeedBack Received");
        }

        [HttpGet, Route("GetAllPending")]
        public async Task<IActionResult> GetAllPendingIssuesAsync()
        {
            var listIssuePending = await context.issues
                .Include(x => x.author)
                .Include(x => x.category)
                .Where(X => X.status.Trim().ToLower().Equals(IssueStatus.pending))
                .ToListAsync();
            var listIssuePendingDto = new List<IssueDto>();
            foreach (var issue in listIssuePending)
            {
                var issueDto = mapper.Map<IssueDto>(issue);
                issueDto.authorName = issue.author.UserName;
                issueDto.cateName = issue.category.cateName;
                listIssuePendingDto.Add(issueDto);
            }

            if (listIssuePendingDto.Count.Equals(0))
            {
                return NoContent();
            }
            return Ok(listIssuePendingDto);
        }

        [HttpPost, Route("CreateIssue")]
        public async Task<IActionResult> CreateIssueAsync([FromForm] CreateIssueDto dto, List<IFormFile> listFiles, [FromHeader] string Authorization)
        {
            if (ModelState.IsValid)
            {
                var listFileError = new List<string>();
                //Decode Token for Author
                var author = await tokenService.DecodeToken(Authorization);
                if (author is null)
                {
                    return BadRequest("Authorization Is Invalid!");
                }

                var newIssue = mapper.Map<Issues>(dto);
                newIssue.authorId = author.Id;
                newIssue.cateId = Guid.Parse(dto.cateId);
                newIssue.status = IssueStatus.pending;
                await context.issues.AddAsync(newIssue);
                await context.SaveChangesAsync();

                if (!listFiles.Count.Equals(0))
                {
                    foreach (var formFile in listFiles)
                    {
                        var fileResult = await fileService.UploadFile(formFile, newIssue.ID.ToString(), config["FilePaths:IssueFiles"]);
                        if (!fileResult.isSucceeded)
                        {
                            listFileError.Add(formFile.Name);
                        }
                    }
                }

                if (!listFileError.Count.Equals(0))
                {
                    return new JsonResult(new 
                    {
                        message = "Issue created but some files cannot be uploaded: ", 
                        listFileError 
                    }) { StatusCode = 200 };
                }

                //Send Email Noti to Block Manager
                var authorBlockId = await context.userHistories
                    .Include(x => x.apartment)
                    .Where(x => x.userId.Equals(Guid.Parse(author.Id)))
                    .Select(x => x.apartment.blockId)
                    .FirstOrDefaultAsync();
                var blockManagerEmail = await context.blocks
                    .Where(x => x.blockId.Equals(authorBlockId))
                    .Select(x => x.blockManager.Email)
                    .FirstOrDefaultAsync();

                var mailContent = new MailContent()
                {
                    ToEmail = blockManagerEmail,
                    Subject = $"Citizen {author.UserName} raised an issue",
                    Body = $"Citizen {author.UserName} has created an issue {newIssue.title} under category {newIssue.category.cateName}"
                };

                await mailService.SendMailAsync(mailContent);

                return Ok("Issue Created");
            }
            return BadRequest("Error");
        }

        [HttpPut, Route("EditIssue")]
        public async Task<IActionResult> EditIssueAsync(string issueId, CreateIssueDto dto)
        {
            if (issueId is null)
            {
                return BadRequest("Issue ID is null");
            }
            var existIssue = await context.issues
                .FirstOrDefaultAsync(x => x.ID.Equals(Guid.Parse(issueId)));
            if (existIssue is null)
                return BadRequest("No Issue found");

            mapper.Map(dto, existIssue);
            existIssue.lastModifiedDate = DateOnly.FromDateTime(DateTime.UtcNow);
            try
            {
                context.issues.Update(existIssue);
                await context.SaveChangesAsync();
                return Ok($"Issue {existIssue.title} Updated");
            }
            catch (Exception ex)
            {
                logger.LogError(ex.Message);
                return BadRequest($"Error in Update Issue: {ex.Message}");
            }
        }

        [HttpDelete, Route("DeleteIssue")]
        public async Task<IActionResult> DeleteIssue(string issueId)
        {
            if (issueId is null)
            {
                return BadRequest("Issue ID is null");
            }

            var existIssue = await context.issues.FirstOrDefaultAsync(x => x.ID.Equals(Guid.Parse(issueId)));
            if (existIssue is null)
            {
                return BadRequest("Issue Not Found");
            }
            try
            {
                context.issues.Remove(existIssue);
                await context.SaveChangesAsync();
                return Ok($"Issue {existIssue?.title} deleted");
            }
            catch (Exception ex)
            {
                logger.LogError(ex.Message);
                return BadRequest($"Error in deleting Issue {existIssue.title}: {ex.Message}");
            }
        }
    }
}