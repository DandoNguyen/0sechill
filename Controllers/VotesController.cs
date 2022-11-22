using _0sechill.Data;
using _0sechill.Dto.Votes.Response;
using _0sechill.Models;
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
    [Authorize(AuthenticationSchemes =JwtBearerDefaults.AuthenticationScheme)]
    public class VotesController : ControllerBase
    {
        private readonly UserManager<ApplicationUser> userManager;
        private readonly ApiDbContext context;

        public VotesController(
            UserManager<ApplicationUser> userManager,
            ApiDbContext context)
        {
            this.userManager = userManager;
            this.context = context;
        }

        /// <summary>
        /// this is the method that get all votes of an issue
        /// </summary>
        /// <param name="issueID">this is the ID string of the issue</param>
        /// <returns>http Request</returns>
        [HttpGet, Route("GetAllVotes")]
        public async Task<IActionResult> GetAllVotes(string issueID)
        {
            var issue = await context.issues
                .Where(x => x.ID.Equals(Guid.Parse(issueID)))
                .Include(x => x.votes).ThenInclude(x => x.User)
                .FirstOrDefaultAsync();
            if (issue is null) return NotFound();

            var result = new IssueVotesDto();
            foreach (var vote in issue.votes)
            {
                if (vote.IsVoteUp)
                {
                    result.voteUpCount++;
                    result.listUserUp.Add(vote.User.lastName + vote.User.firstName);
                }
                else
                {
                    result.voteDownCount++;
                    result.listUserDown.Add(vote.User.lastName + vote.User.firstName);
                }
            }

            return Ok(result);
        }
    }
}
