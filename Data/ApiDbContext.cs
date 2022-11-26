using _0sechill.Hubs.Model;
using _0sechill.Models;
using _0sechill.Models.Account;
using _0sechill.Models.Infrastructure;
using _0sechill.Models.IssueManagement;
using _0sechill.Models.LookUpData;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System.Text.RegularExpressions;

namespace _0sechill.Data
{
    public class ApiDbContext : IdentityDbContext<ApplicationUser>
    {
        public ApiDbContext(DbContextOptions<ApiDbContext> options) : base(options)
        {
        }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<ApplicationUser>()
                .HasOne(a => a.block)
                .WithOne(b => b.blockManager)
                .HasForeignKey<Block>(b => b.blockManagerId);

            //modelBuilder.Entity<Issues>()
            //    .HasOne(a => a.assignIssue)
            //    .WithOne(b => b.Issue)
            //    .HasForeignKey<AssignIssue>(b => b.issueId);


            base.OnModelCreating(modelBuilder);
        }

        public DbSet<Vote> votes { get; set; }
        public DbSet<BookingTask> bookingTasks { get; set; }
        public DbSet<PublicFacility> publicFacilities { get; set; }
        public DbSet<LookUpTable> lookUp { get; set; }
        public DbSet<Notifications> notifications { get; set; }
        public DbSet<Room> chatRooms { get; set; }
        public DbSet<Message> chatMessages { get; set; }
        public DbSet<AssignIssue> assignIssues { get; set; }
        public DbSet<ApplicationUser> ApplicationUser { get; set; }
        public DbSet<Department> departments { get; set; }
        public DbSet<Comments> comments { get; set; }
        public DbSet<Apartment> apartments { get; set; }
        public DbSet<Block> blocks { get; set; }
        public DbSet<UserHistory> userHistories { get; set; }
        public DbSet<RentalHistory> rentalHistories { get; set; }
        public DbSet<SocialRecognization> socialRecognizations { get; set; }
        public DbSet<Issues> issues { get; set; }
        public DbSet<Models.IssueManagement.FilePath> filePaths { get; set; }
        public DbSet<Category> categories { get; set; }
    }
}
