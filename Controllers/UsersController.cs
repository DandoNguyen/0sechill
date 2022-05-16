using _0sechill.Data;
using _0sechill.Repository;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace _0sechill.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        private IRepoWrapper repo;

        public UsersController(IRepoWrapper repo)
        {
            this.repo = repo;
        }
        [HttpGet]
        public IActionResult GetAllUsers()
        {
            var userList = repo.ApplicationUser.FindAll();
            if (userList.Count().Equals(0))
            {
                return NoContent();
            }
            return Ok(userList);
        }
    }
}
