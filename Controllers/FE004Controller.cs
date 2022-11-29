using _0sechill.Data;
using _0sechill.Dto.FE004.Response;
using _0sechill.Models;
using AutoMapper;
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
        private readonly IMapper mapper;

        public FE004Controller(
            ApiDbContext context,
            UserManager<ApplicationUser> userManager,
            RoleManager<IdentityRole> roleManager,
            IMapper mapper)
        {
            this.context = context;
            this.userManager = userManager;
            this.roleManager = roleManager;
            this.mapper = mapper;
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
                return BadRequest("Start Date must be on Monday");
            }

            var listBookingDate = new List<BookingTask>();
            listBookingDate = await context.bookingTasks.Where(x => x.DateOfBooking.ToDateTime(TimeOnly.MinValue).Ticks <= DateTo.Ticks
            && x.DateOfBooking.ToDateTime(TimeOnly.MinValue).Ticks >= DateFrom.Ticks)
                .ToListAsync();

            return Ok(listBookingDate);
        }

        [HttpGet, Route("GetAllBooking")]
        public async Task<IActionResult> GetAllBooking()
        {
            var listResult = new List<BookingTaskDto>();
            var listBooking = await context.bookingTasks
                .Include(x => x.PublicFacility)
                .Include(x => x.User)
                .ToListAsync();

            foreach (var bookingTask in listBooking)
            {
                var bookingDto = new BookingTaskDto();
                bookingDto = mapper.Map<BookingTaskDto>(bookingTask);
                bookingDto.UserName = bookingTask.User.UserName;
                bookingDto.DateAndTimeOfBooking = bookingTask.DateOfBooking.ToDateTime(bookingTask.TimeLevelOfBooking);
                bookingDto.listFacil.Add(bookingTask.PublicFacility.typeFacil + " - " + bookingTask.PublicFacility.facilCode);
                listResult.Add(bookingDto);
            }

            return Ok(listResult);
        }

        [HttpGet, Route("GetMyBooking")]
        public async Task<IActionResult> GetMyBookkingTask()
        {
            var listResult = new List<BookingTaskDto>();

            var listMyBooking = new List<BookingTask>();
            listMyBooking = await context.bookingTasks
                .Include(x => x.PublicFacility)
                .Include(x => x.User)
                .Where(x => x.userID.Equals(this.User.FindFirst("ID").Value))
                .ToListAsync();

            if (!listMyBooking.Any())
            {
                return Ok($"No Booking Task user id {this.User.FindFirst("ID").Value}");
            }

            foreach (var bookingTask in listMyBooking)
            {
                var bookingDto = new BookingTaskDto();
                bookingDto = mapper.Map<BookingTaskDto>(bookingTask);
                bookingDto.UserName = bookingTask.User.UserName;
                bookingDto.DateAndTimeOfBooking = bookingTask.DateOfBooking.ToDateTime(bookingTask.TimeLevelOfBooking);
                bookingDto.listFacil.Add(bookingTask.PublicFacility.typeFacil + " - " + bookingTask.PublicFacility.facilCode);

                listResult.Add(bookingDto);
            }

            return Ok(listResult);
        }

        /// <summary>
        /// this is the method that create a new booking task
        /// </summary>
        /// <param name="bookingTaskID">the ID string of the bookingTask</param>
        /// <param name="facilityID">the ID string of the facility</param>
        /// <param name="dateTimeOfBooking">the date and Time of the booking task</param>
        /// <returns></returns>
        [HttpPost, Route("AddNewBookingTask")]
        public async Task<IActionResult> addNewBookingTask(string facilityID, DateTime dateTimeOfBooking)
        {
            var user = await userManager.FindByIdAsync(this.User.FindFirst("ID").Value);
            if (user is null)
            {
                return Unauthorized();
            }

            var facility = await context.publicFacilities.FindAsync(Guid.Parse(facilityID));
            if (facility is null)
                return BadRequest("Facility Not Found!");

            var newBookingTask = new BookingTask();
            newBookingTask.isAvailable = false;
            newBookingTask.PublicFacility = facility;
            newBookingTask.userID = user.Id;
            newBookingTask.User = user;
            newBookingTask.DateOfBooking = DateOnly.FromDateTime(dateTimeOfBooking);
            newBookingTask.TimeLevelOfBooking = TimeOnly.FromDateTime(dateTimeOfBooking);

            if (ModelState.IsValid)
            {
                await context.bookingTasks.AddAsync(newBookingTask);
                await context.SaveChangesAsync();
                return Ok("Booking Task received!");
            }

            return new JsonResult("Can't receive Booking Task") { StatusCode = 500 };
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
