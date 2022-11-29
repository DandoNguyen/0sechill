using _0sechill.Data;
using _0sechill.Dto.Comments.Request;
using _0sechill.Dto.Comments.Response;
using _0sechill.Models;
using _0sechill.Models.IssueManagement;
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
    public class CommentController : ControllerBase
    {
        private readonly ApiDbContext context;
        private readonly ITokenService tokenService;
        private readonly ILogger<CommentController> logger;
        private readonly IMapper mapper;
        private readonly UserManager<ApplicationUser> userManager;

        public CommentController(
            ApiDbContext context,
            ITokenService tokenService,
            ILogger<CommentController> logger,
            IMapper mapper,
            UserManager<ApplicationUser> userManager)
        {
            this.context = context;
            this.tokenService = tokenService;
            this.logger = logger;
            this.mapper = mapper;
            this.userManager = userManager;
        }

        /// <summary>
        /// get list of available comments for a specific issue
        /// </summary>
        /// <param name="issueId"></param>
        /// <returns></returns>
        [HttpGet, Route("GetComment")]
        public async Task<IActionResult> GetCommentOfIssue(string issueID)
        {
            if (issueID is null)
            {
                return BadRequest("Issue Id must not be null");
            }

            var existIssue = await context.issues.FirstOrDefaultAsync(x => x.ID.Equals(Guid.Parse(issueID)));
            if (existIssue is null)
            {
                return BadRequest("Issue not found");
            }

            var listComments = await context.comments
                .Where(x => x.issueId.Equals(existIssue.ID))
                .Where(x => x.isChild.Equals(false))
                .ToListAsync();
            var listCommentDto = new List<CommentDto>();
            foreach (var comment in listComments)
            {
                var commentDto = mapper.Map<CommentDto>(comment);
                var user = await userManager.FindByIdAsync(comment.authorId);
                commentDto.authorName = user.UserName;
                commentDto.childComments = await CheckForChildCommentAsync(commentDto.ID.ToString(), comment.authorId.ToString());
                listCommentDto.Add(commentDto);
            }
            return Ok(listCommentDto);
        }

        /// <summary>
        /// creating comment for a specific issue
        /// </summary>
        /// <param name="dto"></param>
        /// <param name="Authorization"></param>
        /// <returns></returns>
        [HttpPost, Route("CreateComment")]
        public async Task<IActionResult> CreateComment(CreateCommentDto dto)
        {

            var author = await userManager.FindByIdAsync(this.User.FindFirst("ID").Value);

            var newComment = new Comments();
            if (dto.isChild)
            {
                if (string.IsNullOrEmpty(dto.parent_ID))
                {
                    return BadRequest("Reply Comment doesn't have parent comment Id");
                }
                newComment = mapper.Map<Comments>(dto);
                newComment.authorId = author.Id;
                newComment.parentId = Guid.Parse(dto.parent_ID);
            }
            else
            {
                newComment = mapper.Map<Comments>(dto);
                newComment.authorId = author.Id;
            }            

            if (ModelState.IsValid)
            {
                try
                {
                    await context.comments.AddAsync(newComment);
                    await context.SaveChangesAsync();
                    return Ok("Comment Added");
                }
                catch (Exception ex)
                {
                    logger.LogError(ex.Message);
                    return BadRequest("Error in Creating new comment");
                }
            }

            return BadRequest("Invalid Payload");
        }

        /// <summary>
        /// editing existing comment
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        [HttpPut, Route("EditComment")]
        public async Task<IActionResult> EditCommnetAsync(EditCommentDto dto)
        {
            var existComment = await context.comments.FindAsync(Guid.Parse(dto.commentId));
            if (existComment is null)
            {
                return BadRequest("Comment not Found");
            }
            existComment.content = dto.newContent;
            context.comments.Update(existComment);
            await context.SaveChangesAsync();
            return Ok("Comment Edited");
        }

        /// <summary>
        /// deleting existing comment
        /// </summary>
        /// <param name="commentId"></param>
        /// <param name="Authorization"></param>
        /// <returns></returns>
        [HttpDelete, Route("DeleteComment")]
        public async Task<IActionResult> DeleteCommentAsync(string commentId)
        {

            var existComment = await context.comments
                .Where(x => x.ID.Equals(Guid.Parse(commentId)))
                .Include(x => x.authors)
                .FirstOrDefaultAsync();
            if (existComment is null)
            {
                return BadRequest("Comment Not Found");
            }

            //Delete Child Comment if any
            if (!existComment.isChild)
            {
                var listChildComment = await context.comments
                    .Where(x => x.parentId.Equals(Guid.Parse(commentId)))
                    .ToListAsync();
                if (!listChildComment.Count.Equals(0))
                    context.comments.RemoveRange(listChildComment);
            }

            var loggedUser = await userManager.GetUserAsync(this.User);
            if (!loggedUser.Id.Equals(existComment.authors.Id))
            {
                return Unauthorized("Only Author can delete this comment");
            }

            context.comments.Remove(existComment);
            await context.SaveChangesAsync();
            return Ok("Comment Deleted");
        }

        /// <summary>
        /// private function check for relation comment => see the references
        /// </summary>
        /// <param name="parentId"></param>
        /// <param name="authorId"></param>
        /// <returns></returns>
        private async Task<List<CommentDto>> CheckForChildCommentAsync(string parentId, string authorId)
        {
            var listChildComment = await context.comments
                .Where(x => x.parentId.Equals(parentId))
                .ToListAsync();
            if (listChildComment.Count.Equals(0))
            {
                return null;
            }

            var listChildCommentDto = new List<CommentDto>();
            foreach (var comment in listChildComment)
            {
                var childCommentDto = mapper.Map<CommentDto>(comment);
                childCommentDto.authorName = await context.ApplicationUser
                    .Where(x => x.Id.Equals(authorId))
                    .Select(x => x.UserName)
                    .FirstOrDefaultAsync();
                listChildCommentDto.Add(childCommentDto);
            }
            return listChildCommentDto;
        }
    }
}
