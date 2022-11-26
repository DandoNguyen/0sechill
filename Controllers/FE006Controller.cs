using _0sechill.Data;
using _0sechill.Dto.FE001.Response;
using _0sechill.Dto.FE003.Response;
using _0sechill.Dto.FE006.Request;
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
using System.Security.Claims;

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
        private readonly string STATUS_LOOKUP_CODE = "05";
        private readonly string STATUS_NEW_LOOKUP_INDEX = "03";
        private readonly string STATUS_PENDING_REVIEW_INDEX = "01";
        private readonly string STATUS_IN_PROGRESS_INDEX = "04";

        public FE006Controller(
            ApiDbContext context,
            UserManager<ApplicationUser> userManager,
            RoleManager<IdentityRole> roleManager,
            IMapper mapper,
            IMailService mailService)
        {
            this.context = context;
            this.userManager = userManager;
            this.roleManager = roleManager;
            this.mapper = mapper;
            this.mailService = mailService;
        }

        /// <summary>
        /// this is the endpoints for manager and admin to get all new issue
        /// </summary>
        /// <returns></returns>
        [Authorize(Roles = "admin, blockManager")]
        [HttpGet, Route("GetAllNewIssue")]
        public async Task<IActionResult> getAllNewIssue()
        {
            var statusNew = await context.lookUp
                .Where(x => x.lookUpTypeCode.Equals(STATUS_LOOKUP_CODE))
                .Where(x => x.index.Equals(STATUS_NEW_LOOKUP_INDEX))
                .FirstOrDefaultAsync();

            var listIssue = new List<Issues>();
            listIssue = await context.issues
                .Where(x => x.status.Equals(statusNew)).ToListAsync();

            var listIssueDto = new List<IssueDto>();
            if (listIssue.Any())
            {
                foreach (var issue in listIssue)
                {
                    var newIssueDto = new IssueDto();
                    newIssueDto = mapper.Map<IssueDto>(issue);
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
                .Where(x => x.issueId.Equals(issue))
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
        public async Task<IActionResult> GetAllIssue()
        {
            var loggedInUser = await userManager.FindByIdAsync(this.User.FindFirst("ID").Value);
            if (loggedInUser is null)
            {
                return Unauthorized();
            }

            var listAssigned = await context.assignIssues
                .Include(x => x.Issue)
                .Where(x => x.staffId.Equals(loggedInUser.Id))
                .Select(x => x.Issue).ToListAsync();

            var listResult = new List<IssueDto>();
            foreach (var issue in listAssigned)
            {
                var issueDto = new IssueDto();
                issueDto = mapper.Map<IssueDto>(issue);
                listResult.Add(issueDto);
            }

            return Ok(listAssigned);
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

            newAssignIssue.issueId = issue.ID.ToString();
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
        [HttpGet, Route("FilterSearchIssues")]
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
