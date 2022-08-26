using _0sechill.Data;
using _0sechill.Dto.Cate.Request;
using _0sechill.Dto.Cate.Response;
using _0sechill.Dto.Issues.Response;
using _0sechill.Models.IssueManagement;
using AutoMapper;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace _0sechill.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public class CategoryController : ControllerBase
    {
        private readonly ApiDbContext context;
        private readonly IMapper mapper;
        private readonly ILogger<CategoryController> logger;

        public CategoryController(
            ApiDbContext context,
            IMapper mapper,
            ILogger<CategoryController> logger)
        {
            this.context = context;
            this.mapper = mapper;
            this.logger = logger;
        }

        /// <summary>
        /// getting a list of available categories
        /// </summary>
        /// <returns></returns>
        [HttpGet, Route("GetAllCate")]
        public async Task<IActionResult> GetAllCate()
        {
            var listCate = await context.categories.ToListAsync();
            if (listCate.Count.Equals(0))
            {
                return BadRequest("No Category available");
            }

            var listCateDto = new List<CateDto>();
            foreach (var cate in listCate)
            {
                var cateDto = mapper.Map<CateDto>(cate);
                listCateDto.Add(cateDto);
            }
            return Ok(listCateDto);
        } 

        /// <summary>
        /// Getting a list of avalable issues in specific category
        /// </summary>
        /// <param name="cateId"></param>
        /// <returns></returns>
        [HttpGet, Route("GetAllIssue")]
        public async Task<IActionResult> GetAllIssueInCateAsync(string cateId)
        {
            var existCate = await context.categories.FirstOrDefaultAsync(x => x.ID.Equals(Guid.Parse(cateId)));
            if (existCate == null)
                return BadRequest($"Category not Found");
            
            var listIssueDto = new List<IssueDto>();
            var listExistIssue = await context.issues
                .Where(x => x.cateId.Equals(cateId))
                .Include(x => x.author)
                .Include(x => x.category)
                .ToListAsync();
            if (listExistIssue.Count.Equals(0))
            {
                return BadRequest($"No Issue found in Category {existCate.cateName}");
            }
            foreach (var issue in listExistIssue)
            {
                var issueDto = mapper.Map<IssueDto>(issue);
                issueDto.cateName = issue.category.cateName;
                issueDto.authorName = $"{issue.author.firstName} {issue.author.lastName}";
                listIssueDto.Add(issueDto);
            }
            return Ok(listIssueDto);
        }

        /// <summary>
        /// creating new category
        /// </summary>
        /// <param name="cateName"></param>
        /// <returns></returns>
        [HttpPost, Route("CreateCate")]
        public async Task<IActionResult> CreateCateAsync(string cateName)
        {
            var existCate = await context.categories.FirstOrDefaultAsync(x => x.cateName.ToLower().Equals(cateName.ToLower()));
            if (existCate is not null)
                return BadRequest($"There has been a category named {cateName}");
            var newCate = new Category();
            newCate.cateName = cateName;
            if (ModelState.IsValid)
            {
                try
                {
                    await context.categories.AddAsync(newCate);
                    await context.SaveChangesAsync();
                    return Ok($"New Category {cateName} has been created");
                }
                catch (Exception ex)
                {
                    logger.LogError(ex.Message);
                    return BadRequest($"Error in trying to add new Category {cateName}: {ex.Message}");
                }
            }
            return BadRequest("Invalid Payload");
        }

        /// <summary>
        /// editing existing category
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        [HttpPut, Route("EditCategory")]
        public async Task<IActionResult> EditCateAsync([FromBody] EditCateDto dto)
        {
            var existCate = await context.categories.FirstOrDefaultAsync(x => x.ID.Equals(Guid.Parse(dto.cateId)));
            if (existCate is null)
            {
                return BadRequest($"No Cate found");
            }
            
            existCate.cateName = dto.cateName;
            try
            {
                context.categories.Update(existCate);
                await context.SaveChangesAsync();
                return Ok($"Cate {dto.cateName} has been updated");
            }
            catch (Exception ex)
            {
                logger.LogError(ex.Message);
                return BadRequest($"Error in edit Category {existCate.cateName}: {ex.Message}");
            }
        }
    }
}
