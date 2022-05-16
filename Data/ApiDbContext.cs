using _0sechill.Models;
using Microsoft.EntityFrameworkCore;

namespace _0sechill.Data
{
    public class ApiDbContext : DbContext
    {
        public ApiDbContext(DbContextOptions<ApiDbContext> options) : base(options)
        {
        }

        public DbSet<Users> Users { get; set; }
    }
}
