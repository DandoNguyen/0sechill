using _0sechill.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace _0sechill.Data
{
    public class ApiDbContext : IdentityDbContext<ApplicationUser>
    {
        public ApiDbContext(DbContextOptions<ApiDbContext> options) : base(options)
        {
        }

        public DbSet<ApplicationUser> ApplicationUser { get; set; }
        public DbSet<Apartment> apartments { get; set; }
        public DbSet<Block> blocks { get; set; }
        public DbSet<UserHistory> userHistories { get; set; }
        public DbSet<RentalHistory> rentalHistories { get; set; }
        public DbSet<SocialRecognization> socialRecognizations { get; set; }
    }
}
