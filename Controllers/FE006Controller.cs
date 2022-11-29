using _0sechill.Data;
using _0sechill.Dto.FE001.Response;
using _0sechill.Dto.FE003.Response;
using _0sechill.Dto.FE006.Request;
using _0sechill.Dto.FE006.Response;
using _0sechill.Dto.MailDto;
using _0sechill.Models;
using _0sechill.Models.IssueManagement;
using _0sechill.Services;
using _0sechill.Static;
using AutoMapper;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.Drawing;
using System.Security.Claims;
using ResponseStaffFeedbackDto = _0sechill.Dto.FE006.Response.ResponseStaffFeedbackDto;

namespace _0sechill.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = "admin, staffst, staffbt, blockManager")]
    public class FE006Controller : ControllerBase
    {
        private readonly ApiDbContext context;
        private readonly UserManager<ApplicationUser> userManager;
        private readonly RoleManager<IdentityRole> roleManager;
        private readonly IMapper mapper;
        private readonly IMailService mailService;
        private readonly IFileHandlingService fileHandlingService;
        private readonly IConfiguration config;
        private readonly string STATUS_LOOKUP_CODE = "05";
        private readonly string STATUS_NEW_LOOKUP_INDEX = "03";
        private readonly string STATUS_PENDING_REVIEW_INDEX = "01";
        private readonly string STATUS_IN_PROGRESS_INDEX = "04";
        private readonly string STATUS_DONE_INDEX = "02";

        public FE006Controller(
            ApiDbContext context,
            UserManager<ApplicationUser> userManager,
            RoleManager<IdentityRole> roleManager,
            IMapper mapper,
            IMailService mailService,
            IFileHandlingService fileHandlingService, 
            IConfiguration config)
        {
            this.context = context;
            this.userManager = userManager;
            this.roleManager = roleManager;
            this.mapper = mapper;
            this.mailService = mailService;
            this.fileHandlingService = fileHandlingService;
            this.config = config;
        }

        /// <summary>
        /// this is the endpoint for admin or manager to check feedback from staff
        /// </summary>
        /// <param name="assignIssueID"></param>
        /// <param name="isConfirmed"></param>
        /// <returns></returns>
        [HttpPost, Route("AdminConfirmResult")]
        [Authorize(Roles = "admin, blockManager")]
        public async Task<IActionResult> AdminConfirmResult(string assignIssueID, bool isConfirmed)
        {
            var existAssignedIssue = await context.assignIssues
                .Include(x => x.Issue)
                .Where(x => x.ID.Equals(Guid.Parse(assignIssueID)))
                .FirstOrDefaultAsync();
            if (existAssignedIssue is null)
            {
                return BadRequest("Issue not found");
            }

            existAssignedIssue.isConfirmedByAdmin = isConfirmed;
            if (isConfirmed)
            {
                var doneStatusString = await context.lookUp
                    .Where(x => x.lookUpTypeCode.Equals(STATUS_LOOKUP_CODE))
                    .Where(x => x.index.Equals(STATUS_DONE_INDEX))
                    .Select(x => x.valueString).FirstOrDefaultAsync();
                existAssignedIssue.Issue.status = doneStatusString is null ? "done" : doneStatusString;
                context.assignIssues.Update(existAssignedIssue);
                await context.SaveChangesAsync();
                return Ok("Feedback Confirmed");
            }
            else
            {
                existAssignedIssue.isResolved = isConfirmed;
                context.assignIssues.Update(existAssignedIssue);
                await context.SaveChangesAsync();
                return Ok("Feedback Rejected");
            }
        }

        /// <summary>
        /// this is the endpoint for admin and user to get all resolved Assigned issues
        /// </summary>
        /// <returns></returns>
        [HttpGet, Route("GetAllResolvedIssue")]
        [Authorize(Roles = "admin, blockManager")]
        public async Task<IActionResult> getAllResolvedIssue()
        {
            var listAssignIssue = new List<AssignIssue>();
            listAssignIssue = await context.assignIssues
                .Include(x => x.Issue)
                .Include(x => x.staff)
                .Where(x => x.isResolved.Equals(true))
                .ToListAsync();
            var listResult = new List<IssueStaffDto>();
            if (listAssignIssue.Any())
            {
                foreach (var assignIssue in listAssignIssue)
                {
                    var issueStaffDto = new IssueStaffDto();
                    issueStaffDto = mapper.Map<IssueStaffDto>(assignIssue);
                    issueStaffDto.listFileFromStaff = await fileHandlingService.getListPaths(assignIssue.ID.ToString());
                    listResult.Add(issueStaffDto);
                }
            }

            return Ok(listResult);
        }

        /// <summary>
        /// this is the endpoints to add staff feedback to assigned issue
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        [HttpPost, Route("AddStaffFeedback")]
        public async Task<IActionResult> AddStaffFeedBack([FromForm] StaffFeedbackDto dto)
        {
            var staff = await userManager.FindByIdAsync(User.FindFirst("ID").Value);
            var existAssignIssue = await context.assignIssues
                .Include(x => x.Issue)
                .Include(x => x.staff)
                .Where(x => x.Issue.ID.Equals(Guid.Parse(dto.assignIssueID))).FirstOrDefaultAsync();

            if (!staff.Id.Equals(existAssignIssue.staff.Id))
            {
                return Unauthorized();
            }

            var isSucceed = await updateStaffFeedback(dto, existAssignIssue);
            if (isSucceed)
            {
                
                return Ok("Update Succeed");
            }

            return Ok("Update Fail");
        }

        /// <summary>
        /// this is the private function that update staff feedback and upload file
        /// </summary>
        /// <param name="dto"></param>
        /// <param name="existAssignIssue"></param>
        /// <returns></returns>
        private async Task<bool> updateStaffFeedback(StaffFeedbackDto dto, AssignIssue existAssignIssue)
        {
            existAssignIssue.staffFeedback = dto.staffFeedback;
            existAssignIssue.isResolved = true;
            context.assignIssues.Update(existAssignIssue);
            foreach (var file in dto.listFiles)
            {
                try
                {
                    await fileHandlingService.UploadFile(file, existAssignIssue.ID.ToString(), config["FilePaths:AssignIssueResult"]);
                }
                catch (Exception)
                {
                    return false;
                }
            }
            await context.SaveChangesAsync();
            return true;
        }

        [HttpPut, Route("ChangePriority")]
        public async Task<IActionResult> changePriority(string AssignIssueId, int priorityLevel)
        {
            var assignIssue = await context.assignIssues
                .Include(x => x.Issue)
                .Where(x => x.ID.Equals(Guid.Parse(AssignIssueId))).FirstOrDefaultAsync();
            if (assignIssue is null)
            {
                return BadRequest("Assign Issue not found");
            }

            assignIssue.Issue.priorityLevel = priorityLevel;
            context.assignIssues.Update(assignIssue);
            await context.SaveChangesAsync();
            return Ok($"Issue {assignIssue.Issue.title} has been set to priority: {priorityLevel}");
        }

        [HttpGet, Route("GetStaffFeedback")]
        public async Task<IActionResult> GetStaffFeedback(string issueId)
        {
            var assignIssue = await context.assignIssues
                .Include(x => x.Issue)
                .Where(x => x.Issue.ID.Equals(Guid.Parse(issueId)))
                .FirstOrDefaultAsync();

            if (assignIssue is null)
            {
                return BadRequest("Assign Issue not found");
            }

            var staffFeedbackDto = new ResponseStaffFeedbackDto();
            staffFeedbackDto.staffFeedback = assignIssue.staffFeedback;
            staffFeedbackDto.listFileID = await fileHandlingService.getListPaths(assignIssue.ID.ToString());
            return Ok(staffFeedbackDto);
        }

        /// <summary>
        /// this is the endpoints for manager and admin to get all new issue
        /// </summary>
        /// <returns></returns>
        [Authorize(Roles = "admin, blockManager")]
        [HttpGet, Route("GetAllNewIssue")]
        public async Task<IActionResult> getAllNewIssue()
        {
            var listIssue = new List<Issues>();
            listIssue = await context.issues
                .Include(x => x.listCateLookUp)
                .Include(x => x.author)
                .ToListAsync();

            var listIssueDto = new List<IssueDto>();
            if (listIssue.Any())
            {
                foreach (var issue in listIssue)
                {
                    var newIssueDto = new IssueDto();
                    newIssueDto = mapper.Map<IssueDto>(issue);
                    newIssueDto.authorName = issue.author.UserName;

                    foreach (var cate in issue.listCateLookUp)
                    {
                        newIssueDto.listCategory.Add(cate.valueString);
                    }

                    listIssueDto.Add(newIssueDto);
                }
            }

            return Ok(listIssueDto);
        }

        [Authorize(Roles = "staffst")]
        [HttpPost, Route("ConfirmIssue")]
        public async Task<IActionResult> confirmIssue([Required] bool isConfirmed, [Required] string issueID)
        {
            var issue = await context.issues
                .Where(x => x.ID.Equals(Guid.Parse(issueID)))
                .FirstOrDefaultAsync();
            if (issue is null)
            {
                return BadRequest("Issue not found");
            }

            var assignIssue = await context.assignIssues
                .Where(x => x.Issue.Equals(issue))
                .FirstOrDefaultAsync();
            var user = await userManager.FindByIdAsync(User.FindFirst("ID").Value);

            if (!assignIssue.staffId.Equals(user.Id))
            {
                return Unauthorized();
            }

            assignIssue.isConfirmed = isConfirmed;

            var statusInProgressString = await context.lookUp
                .Where(x => x.lookUpTypeCode.Equals(STATUS_LOOKUP_CODE))
                .Where(x => x.index.Equals(STATUS_IN_PROGRESS_INDEX))
                .Select(x => x.valueString).FirstOrDefaultAsync();
            issue.status = statusInProgressString;

            try
            {
                context.assignIssues.Update(assignIssue);
                context.issues.Update(issue);
                await context.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                return new JsonResult($"Error: {ex.Message}") { StatusCode = 500 };
            }

            return Ok("Success");
        }

        /// <summary>
        /// this is the method to list all assigned issues
        /// </summary>
        /// <returns></returns>
        [HttpGet, Route("GetMyIssues")]
        [Authorize(Roles = "staffst, staffbt")]
        public async Task<IActionResult> GetAllIssue()
        {
            var loggedInUser = await userManager.FindByIdAsync(this.User.FindFirst("ID").Value);
            if (loggedInUser is null)
            {
                return Unauthorized();
            }

            var listAssigned = await context.assignIssues
                .Include(x => x.Issue)
                .Include(x => x.Issue.author)
                .Where(x => x.staffId.Equals(loggedInUser.Id))
                .Select(x => x.Issue)
                .ToListAsync();

            var listResult = new List<IssueDto>();
            foreach (var issue in listAssigned)
            {
                var issueDto = new IssueDto();
                issueDto = mapper.Map<IssueDto>(issue);
                listResult.Add(issueDto);
            }

            if (!listResult.Any())
            {
                return Ok("No issued Assigned to you");
            }

            return Ok(listResult);
        }

        /// <summary>
        /// this is the endpoint that assign issue to staff from service team
        /// </summary>
        /// <param name="staffID">Id of Staff</param>
        /// <param name="issueID">ID of issue</param>
        /// <returns>HTTP Response</returns>
        [Authorize(Roles = "admin, blockManager")]
        [HttpPost, Route("AssignIssueToStaff")]
        public async Task<IActionResult> AssignIssue(string staffID, string issueID)
        {
            var staff = await userManager.FindByIdAsync(staffID);
            if (staff is null)
            {
                return BadRequest("Staff is not exist");
            }

            var issue = await context.issues
                .Where(x => x.ID.Equals(Guid.Parse(issueID)))
                .FirstOrDefaultAsync();
            if (issue is null)
            {
                return BadRequest("Issue is not exist or has been deleted");
            }

            var newAssignIssue = new AssignIssue();
            
            newAssignIssue.staffId = staff.Id;
            newAssignIssue.staff = staff;

            newAssignIssue.Issue = issue;
            newAssignIssue.isConfirmedByAdmin = true;

            try
            {
                if (ModelState.IsValid)
                {
                    var pendingStatusString = await context.lookUp
                        .Where(x => x.lookUpTypeCode.Equals(STATUS_LOOKUP_CODE))
                        .Where(x => x.index.Equals(STATUS_PENDING_REVIEW_INDEX))
                        .Select(x => x.valueString).FirstOrDefaultAsync();

                    if (!string.IsNullOrEmpty(pendingStatusString))
                    {
                        issue.status = pendingStatusString;
                        context.issues.Update(issue);
                    }

                    await context.assignIssues.AddAsync(newAssignIssue);
                    await context.SaveChangesAsync();
                    await SendMailToStaffSt(staff, User.FindFirst(ClaimTypes.Name).Value);
                    return Ok($"Issue has been assigned to staff {staff.UserName}");
                }
                return BadRequest("Cannot Assign Issue");
            }
            catch (Exception ex)
            {
                return new JsonResult(ex.Message) { StatusCode = 500 };
            }
        }

        /// <summary>
        /// this function is to send mail notification to staff
        /// </summary>
        /// <param name="staff"></param>
        /// <param name="adminUserName"></param>
        /// <returns></returns>
        private async Task<bool> SendMailToStaffSt(ApplicationUser staff, string adminUserName)
        {
            var newMailContent = new MailContent()
            {
                ToEmail = staff.Email,
                Subject = "New Issue",
                Body = $"{adminUserName} has assign an issue to you"
            };

            try
            {
                await mailService.SendMailAsync(newMailContent);
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        /// <summary>
        /// this is the endpoint that get all staff from service team
        /// </summary>
        /// <returns></returns>
        [HttpGet, Route("GetAllServiceTeam")]
        public async Task<IActionResult> getAllStaffAccount()
        {
            var roleStaffSt = UserRole.Staffst;
            var listStaff = new List<ApplicationUser>();
            listStaff = (List<ApplicationUser>) await userManager.GetUsersInRoleAsync(roleStaffSt);
            
            var listResult = new List<UserDto>();
            if (listStaff.Any()) 
            {
                foreach (var staff in listStaff)
                {
                    var UserDto = new UserDto();
                    UserDto = mapper.Map<UserDto>(staff);
                    listResult.Add(UserDto);
                }
            }

            return Ok(listResult);
        }

        /// <summary>
        /// This is the endpoints for filter search
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        [HttpPost, Route("FilterSearchIssues")]
        public async Task<IActionResult> SearchByFilter(SearchIssueFilterDto dto)
        {
            var searchQuery = context.issues.AsQueryable();
            
            if (!string.IsNullOrEmpty(dto.status))
            {
                searchQuery = searchQuery.Where(x => x.status.Equals(dto.status));
            }

            if (dto.priorityLevel is 0)
            {
                searchQuery = searchQuery.Where(x => x.priorityLevel.Equals(dto.priorityLevel));
            }

            searchQuery = searchQuery.Where(x => x.title.Equals(dto.title));

            var listIssues = new List<Issues>();
            var listResult = new List<IssueDto>();
            listIssues = await searchQuery.ToListAsync();

            if (listIssues.Any())
            {
                foreach (var issue in listIssues)
                {
                    var newIssueDto = new IssueDto();
                    newIssueDto = mapper.Map<IssueDto>(issue);
                    listResult.Add(newIssueDto);
                }
            }

            return Ok(listResult);
        }
    }
}
