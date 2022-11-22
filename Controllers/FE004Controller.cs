using _0sechill.Data;
using _0sechill.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace _0sechill.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class FE004Controller : ControllerBase
    {
        private readonly ApiDbContext context;
        private readonly UserManager<ApplicationUser> userManager;
        private readonly RoleManager<IdentityRole> roleManager;

        public FE004Controller(
            ApiDbContext context,
            UserManager<ApplicationUser> userManager,
            RoleManager<IdentityRole> roleManager)
        {
            this.context = context;
            this.userManager = userManager;
            this.roleManager = roleManager;
        }

        /// <summary>
        /// this is the method that list all the facility in the application
        /// </summary>
        /// <returns></returns>
        [HttpGet, Route("GetAllFacility")]
        public async Task<IActionResult> GetListFacilities()
        {
            return Ok(await context.publicFacilities.ToListAsync());
        }

        [HttpGet, Route("GetExistBookingTasks")]
        public async Task<IActionResult> GetExistBookng(DateTime DateFrom, DateTime DateTo)
        {
            var weekDayValue = (int)DateFrom.DayOfWeek;
            if (weekDayValue != 1)
            {
                return BadRequest("Invalid Start Date");
            }

            var listBookingDate = await context.bookingTasks.Where(x => x.DateOfBooking.ToDateTime(TimeOnly.MinValue).Ticks <= DateTo.Ticks
            && x.DateOfBooking.ToDateTime(TimeOnly.MinValue).Ticks >= DateFrom.Ticks)
                .ToListAsync();

            if (!listBookingDate.Any())
            {
                listBookingDate = await createNewSetOfBookingTask(DateFrom, DateTo);
            }

            return Ok(listBookingDate);
        }

        /// <summary>
        /// this is the method that create a new booking task
        /// </summary>
        /// <param name="bookingTaskID">the ID string of the bookingTask</param>
        /// <param name="facilityID">the ID string of the facility</param>
        /// <param name="dateTimeOfBooking">the date and Time of the booking task</param>
        /// <returns></returns>
        [HttpPost, Route("AddNewBookingTask")]
        public async Task<IActionResult> addNewBookingTask(string bookingTaskID, string facilityID, DateTime dateTimeOfBooking)
        {
            var user = await userManager.FindByIdAsync(this.User.FindFirst("ID").Value);
            if (user is null)
            {
                return Unauthorized();
            }

            var facility = await context.publicFacilities.FindAsync(facilityID);
            if (facility is null)
                return BadRequest("Facility Not Found!");

            var existBookingTask = await context.bookingTasks.FindAsync(Guid.Parse(bookingTaskID));
            existBookingTask.isAvailable = false;
            existBookingTask.PublicFacility.Add(facility);
            existBookingTask.userID = user.Id;
            existBookingTask.User = user;
            existBookingTask.DateOfBooking = DateOnly.FromDateTime(dateTimeOfBooking);
            existBookingTask.TimeLevelOfBooking = TimeOnly.FromDateTime(dateTimeOfBooking);

            if (ModelState.IsValid)
            {
                context.bookingTasks.Update(existBookingTask);
                await context.SaveChangesAsync();
                return Ok("Booking Task received!");
            }

            return new JsonResult("Can't receive Booking Task") { StatusCode = 500 };
        }

        /// <summary>
        /// this is the method that create a new set for a given range of date
        /// </summary>
        /// <param name="dateFrom">this is the start date of the date range</param>
        /// <param name="dateTo">this is the end date of the date range</param>
        /// <returns>List<BookingTask></returns>
        private async Task<List<BookingTask>> createNewSetOfBookingTask(DateTime dateFrom, DateTime dateTo)
        {
            var listNewBookingTask = new List<BookingTask>();
            for (DateTime indexDate = dateFrom; indexDate.Ticks <= dateTo.Ticks; indexDate.AddDays(1))
            {
                var newBookingTask = new BookingTask();
                newBookingTask.DateOfBooking = DateOnly.FromDateTime(indexDate);
                listNewBookingTask.Add(newBookingTask);
            }

            foreach (var item in listNewBookingTask)
            {
                if (await context.bookingTasks.FindAsync(item) is not null)
                {
                    context.bookingTasks.Update(item);
                }
                else
                {
                    await context.bookingTasks.AddAsync(item);
                }
            }

            await context.SaveChangesAsync();
            return listNewBookingTask;
        }

        /// <summary>
        /// this is the method used to cancel existing booking task
        /// </summary>
        /// <param name="bookingID"></param>
        /// <returns></returns>
        [HttpDelete, Route("RemoveBookingTask")]
        public async Task<IActionResult> cancelBookingTask(string bookingID)
        {
            var existBookingTask = await context.bookingTasks.Where(x => x.ID.Equals(Guid.Parse(bookingID))).FirstOrDefaultAsync();
            if (existBookingTask is null)
            {
                return BadRequest("Booking Task not Exist");
            }

            var user = await userManager.FindByIdAsync(this.User.FindFirst("ID").Value);
            if (user is null)
            {
                return Unauthorized();
            }

            if (!existBookingTask.User.Id.Equals(user.Id))
            {
                return Unauthorized();
            }

            context.bookingTasks.Remove(existBookingTask);
            await context.SaveChangesAsync();
            return Ok("Booking Task has been cancel");
        }
    }
}
